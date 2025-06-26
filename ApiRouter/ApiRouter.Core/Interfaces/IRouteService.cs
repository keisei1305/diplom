using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ApiRouter.Core.Models;

namespace ApiRouter.Core.Interfaces;

public interface IRouteService
{
    Task<RouteConfig?> GetRouteConfigAsync(string path);
    Task<bool> ValidateRouteAccessAsync(RouteConfig route, HttpContext context);
    Task<string> GetServiceUrlAsync(string path);
} 