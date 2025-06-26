using System.Collections.Generic;

namespace GameService.Core.DTOs
{
    public class CreateGameDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> FilterIds { get; set; }
    }
} 