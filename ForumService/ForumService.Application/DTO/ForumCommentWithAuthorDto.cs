using System;

namespace ForumService.Application.DTO
{
    public class ForumCommentWithAuthorDto
    {
        public int Id { get; set; }
        public int ForumId { get; set; }
        public UserShortDto Author { get; set; } = new UserShortDto();
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
} 