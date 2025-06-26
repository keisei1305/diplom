using System;
using System.Net.Http;
using System.Threading.Tasks;
using ApiRouter.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ApiRouter.Application.Middleware;

public class RouterMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IRouteService _routeService;
    private readonly ILogger<RouterMiddleware> _logger;
    private readonly HttpClient _httpClient;

    public RouterMiddleware(
        RequestDelegate next,
        IRouteService routeService,
        ILogger<RouterMiddleware> logger,
        HttpClient httpClient)
    {
        _next = next;
        _routeService = routeService;
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            var path = context.Request.Path.Value;
            if (string.IsNullOrEmpty(path))
            {
                await _next(context);
                return;
            }

            var route = await _routeService.GetRouteConfigAsync(path);
            if (route == null)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }

            if (!await _routeService.ValidateRouteAccessAsync(route, context))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return;
            }

            var serviceUrl = await _routeService.GetServiceUrlAsync(path);
            var targetUrl = $"{serviceUrl.TrimEnd('/')}/{path.TrimStart('/')}";

            var request = new HttpRequestMessage
            {
                Method = new HttpMethod(context.Request.Method),
                RequestUri = new Uri(targetUrl)
            };

            // Копируем заголовки
            foreach (var header in context.Request.Headers)
            {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
            }

            // Копируем тело запроса
            if (context.Request.Body != null)
            {
                request.Content = new StreamContent(context.Request.Body);
                if (context.Request.ContentType != null)
                {
                    request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(context.Request.ContentType);
                }
            }

            var response = await _httpClient.SendAsync(request);
            
            // Копируем ответ
            context.Response.StatusCode = (int)response.StatusCode;
            foreach (var header in response.Headers)
            {
                context.Response.Headers[header.Key] = header.Value.ToArray();
            }

            await response.Content.CopyToAsync(context.Response.Body);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing request");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
} 