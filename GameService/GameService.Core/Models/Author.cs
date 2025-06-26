using System;
using System.Collections.Generic;

namespace GameService.Core.Models
{
    public class Author
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<Game> Games { get; set; }
    }
} 