using System;

namespace ForumService.Application.DTO
{
    public class ForumWithAuthorDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string GameId { get; set; } = string.Empty;
        public UserShortDto Author { get; set; } = new UserShortDto();
        public ForumCommentWithAuthorDto? LastComment { get; set; }
        public int CommentsCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
} 