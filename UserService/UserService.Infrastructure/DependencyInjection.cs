using UserService.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace UserService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<IMessageConsumer, RabbitMQMessageConsumer>();
        services.AddHostedService<BackgroundMessageConsumer>();
        return services;
    }
} 