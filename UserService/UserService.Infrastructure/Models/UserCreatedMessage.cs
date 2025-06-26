using System.ComponentModel.DataAnnotations;

namespace Shared.Messages.Models;

public class UserCreatedMessage
{
    [Required]
    public string UserId { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string Username { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public string MessageType { get; set; } = "UserCreated";
} 