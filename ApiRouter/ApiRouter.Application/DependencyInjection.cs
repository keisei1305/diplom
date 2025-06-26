using System;
using ApiRouter.Core.Interfaces;
using ApiRouter.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ApiRouter.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddScoped<IRouteService, RouteService>();

        return services;
    }
} 