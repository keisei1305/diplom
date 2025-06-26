using System;
using System.Collections.Generic;

namespace GameService.Core.Models
{
    public class Game
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AuthorId { get; set; }
        public string? GameFileId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Author Author { get; set; }
        public GameFile GameFile { get; set; }
        public ICollection<Filter> Filters { get; set; }
    }
} 