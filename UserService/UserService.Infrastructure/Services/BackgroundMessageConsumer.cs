using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace UserService.Infrastructure.Services;

public class BackgroundMessageConsumer : BackgroundService
{
    private readonly IMessageConsumer _messageConsumer;
    private readonly ILogger<BackgroundMessageConsumer> _logger;

    public BackgroundMessageConsumer(
        IMessageConsumer messageConsumer,
        ILogger<BackgroundMessageConsumer> logger)
    {
        _messageConsumer = messageConsumer;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await _messageConsumer.StartAsync();
            _logger.LogInformation("Message consumer started successfully");

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in background message consumer");
        }
        finally
        {
            await _messageConsumer.StopAsync();
            _logger.LogInformation("Message consumer stopped");
        }
    }
} 