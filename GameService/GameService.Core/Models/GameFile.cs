using System;

namespace GameService.Core.Models
{
    public class GameFile
    {
        public string Id { get; set; }
        public string GameId { get; set; }
        public string OriginalName { get; set; }
        public string Path { get; set; }
        public long Weight { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Game Game { get; set; }
    }
} 