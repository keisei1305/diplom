using UserService.Core.Interfaces;
using UserService.Application.DTO;
using Shared.Messages.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace UserService.Application.Services;

public class MessageHandler : IMessageHandler
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<MessageHandler> _logger;

    public MessageHandler(IServiceScopeFactory serviceScopeFactory, ILogger<MessageHandler> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    public async Task HandleUserCreatedAsync(UserCreatedMessage message)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
        
        try
        {
            var createUserRequest = new CreateUserRequest
            {
                Id = message.UserId,
                Nickname = message.Username,
                Email = message.Email
            };

            await userService.CreateAsync(createUserRequest);
            _logger.LogInformation("User created successfully: {UserId}", message.UserId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user: {UserId}", message.UserId);
            throw;
        }
    }
} 