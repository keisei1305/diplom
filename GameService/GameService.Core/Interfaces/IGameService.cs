using System.Collections.Generic;
using System.Threading.Tasks;
using GameService.Core.DTOs;

namespace GameService.Core.Interfaces
{
    public interface IGameService
    {
        Task<GameDto> GetByIdAsync(string id);
        Task<IEnumerable<GameDto>> GetAllAsync(int page = 1, int pageSize = 10);
        Task<IEnumerable<GameDto>> GetByAuthorIdAsync(string authorId, int page = 1, int pageSize = 10);
        Task<IEnumerable<GameDto>> GetByFilterIdAsync(string filterId, int page = 1, int pageSize = 10);
        Task<IEnumerable<GameDto>> SearchByNameAsync(string searchTerm);
        Task<GameDto> CreateAsync(CreateGameDto createGameDto, string userId);
        Task<GameDto> UpdateAsync(string id, UpdateGameDto updateGameDto);
        Task DeleteAsync(string id);
        Task<(byte[] fileBytes, string fileName)> DownloadGameAsync(string id);
        Task<IEnumerable<GameDto>> GetByAuthorIdAsync(string authorId);
        Task<AuthorDto> GetAuthorByUserIdAsync(string userId);
    }
} 