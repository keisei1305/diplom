using System;
using System.Collections.Generic;

namespace GameService.Core.Models
{
    public class Filter
    {
        public string Id { get; set; }
        public string FilterType { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<Game> Games { get; set; }
    }
} 