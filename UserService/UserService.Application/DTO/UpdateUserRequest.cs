using System.ComponentModel.DataAnnotations;

namespace UserService.Application.DTO
{
    public class UpdateUserRequest
    {
        [Required]
        public string Id { get; set; } = string.Empty;
        [Required]
        public string Nickname { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
} 