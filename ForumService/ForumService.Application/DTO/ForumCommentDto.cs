namespace ForumService.Application.DTO
{
    public class ForumCommentDto
    {
        public int Id { get; set; }
        public int ForumId { get; set; }
        public string AuthorId { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public System.DateTime CreatedAt { get; set; }
    }
} 