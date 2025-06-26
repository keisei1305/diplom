using System.ComponentModel.DataAnnotations;

namespace ForumService.Application.DTO
{
    public class UpdateForumRequest
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string GameId { get; set; } = string.Empty;
    }
} 