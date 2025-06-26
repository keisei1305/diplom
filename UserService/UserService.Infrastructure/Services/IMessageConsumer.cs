namespace UserService.Infrastructure.Services;

public interface IMessageConsumer
{
    Task StartAsync();
    Task StopAsync();
} 