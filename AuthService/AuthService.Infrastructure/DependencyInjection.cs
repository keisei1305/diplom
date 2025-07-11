using AuthService.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<IMessagePublisher, RabbitMQMessagePublisher>();
        return services;
    }
} 