using System;

namespace ForumService.Core.Entities
{
    public class ForumComment
    {
        public int Id { get; set; }
        public int ForumId { get; set; }
        public string AuthorId { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
} 