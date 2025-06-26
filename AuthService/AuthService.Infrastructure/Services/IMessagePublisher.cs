using Shared.Messages.Models;

namespace AuthService.Infrastructure.Services;

public interface IMessagePublisher
{
    Task PublishUserCreatedAsync(UserCreatedMessage message);
} 