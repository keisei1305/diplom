using System.ComponentModel.DataAnnotations;

namespace ForumService.Application.DTO
{
    public class CreateForumCommentRequest
    {
        [Required]
        public int ForumId { get; set; }
        [Required]
        public string AuthorId { get; set; } = string.Empty;
        [Required]
        public string Content { get; set; } = string.Empty;
    }
} 