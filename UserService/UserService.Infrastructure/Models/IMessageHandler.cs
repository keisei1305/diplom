using Shared.Messages.Models;

namespace UserService.Core.Interfaces;
 
public interface IMessageHandler
{
    Task HandleUserCreatedAsync(UserCreatedMessage message);
} 