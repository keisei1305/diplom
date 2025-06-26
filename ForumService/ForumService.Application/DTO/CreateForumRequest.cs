using System.ComponentModel.DataAnnotations;

namespace ForumService.Application.DTO
{
    public class CreateForumRequest
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string GameId { get; set; } = string.Empty;
        [Required]
        public string AuthorId { get; set; } = string.Empty;
    }
} 