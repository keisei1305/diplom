using System;

namespace GameService.Core.DTOs
{
    public class FilterDto
    {
        public string Id { get; set; }
        public string FilterType { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
} 