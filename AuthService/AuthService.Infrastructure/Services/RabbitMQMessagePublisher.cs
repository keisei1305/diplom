using RabbitMQ.Client;
using Shared.Messages.Models;
using Shared.Messages.Constants;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AuthService.Infrastructure.Services;

public class RabbitMQMessagePublisher : IMessagePublisher, IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly ILogger<RabbitMQMessagePublisher> _logger;

    public RabbitMQMessagePublisher(IConfiguration configuration, ILogger<RabbitMQMessagePublisher> logger)
    {
        _logger = logger;
        
        var factory = new ConnectionFactory
        {
            HostName = configuration["RabbitMQ:Host"] ?? "localhost",
            Port = int.Parse(configuration["RabbitMQ:Port"] ?? "5672"),
            UserName = configuration["RabbitMQ:Username"] ?? "guest",
            Password = configuration["RabbitMQ:Password"] ?? "guest"
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        
        // Объявляем очередь
        _channel.QueueDeclare(
            queue: QueueNames.UserCreated,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);
    }

    public Task PublishUserCreatedAsync(UserCreatedMessage message)
    {
        try
        {
            var json = JsonConvert.SerializeObject(message);
            var body = System.Text.Encoding.UTF8.GetBytes(json);

            _channel.BasicPublish(
                exchange: "",
                routingKey: QueueNames.UserCreated,
                basicProperties: null,
                body: body);

            _logger.LogInformation("User created message published for user: {UserId}", message.UserId);
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing user created message for user: {UserId}", message.UserId);
            throw;
        }
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
} 