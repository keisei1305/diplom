using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Messages.Models;
using Shared.Messages.Constants;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UserService.Core.Interfaces;

namespace UserService.Infrastructure.Services;

public class RabbitMQMessageConsumer : IMessageConsumer, IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly ILogger<RabbitMQMessageConsumer> _logger;
    private readonly IMessageHandler _messageHandler;

    public RabbitMQMessageConsumer(
        IConfiguration configuration, 
        ILogger<RabbitMQMessageConsumer> logger,
        IMessageHandler messageHandler)
    {
        _logger = logger;
        _messageHandler = messageHandler;
        
        var factory = new ConnectionFactory
        {
            HostName = configuration["RabbitMQ:Host"] ?? "localhost",
            Port = int.Parse(configuration["RabbitMQ:Port"] ?? "5672"),
            UserName = configuration["RabbitMQ:Username"] ?? "guest",
            Password = configuration["RabbitMQ:Password"] ?? "guest"
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        
        _channel.QueueDeclare(
            queue: QueueNames.UserCreated,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);
    }

    public Task StartAsync()
    {
        var consumer = new EventingBasicConsumer(_channel);
        
        consumer.Received += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = System.Text.Encoding.UTF8.GetString(body);
                
                _logger.LogInformation("Received message: {Message}", message);
                
                var userCreatedMessage = JsonConvert.DeserializeObject<UserCreatedMessage>(message);
                if (userCreatedMessage != null)
                {
                    await ProcessUserCreatedMessage(userCreatedMessage);
                }
                
                _channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message");
                _channel.BasicNack(ea.DeliveryTag, false, true);
            }
        };

        _channel.BasicConsume(
            queue: QueueNames.UserCreated,
            autoAck: false,
            consumer: consumer);

        _logger.LogInformation("RabbitMQ consumer started");
        return Task.CompletedTask;
    }

    public Task StopAsync()
    {
        _channel?.Close();
        _connection?.Close();
        _logger.LogInformation("RabbitMQ consumer stopped");
        return Task.CompletedTask;
    }

    private async Task ProcessUserCreatedMessage(UserCreatedMessage message)
    {
        try
        {
            await _messageHandler.HandleUserCreatedAsync(message);
            _logger.LogInformation("User created in UserService: {UserId}", message.UserId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user in UserService: {UserId}", message.UserId);
            throw;
        }
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
} 