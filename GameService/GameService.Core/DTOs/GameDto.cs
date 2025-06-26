using System;
using System.Collections.Generic;

namespace GameService.Core.DTOs
{
    public class GameDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string GameFileId { get; set; }
        public long FileSize { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<FilterDto> Filters { get; set; }
    }
} 