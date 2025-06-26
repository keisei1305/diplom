using System.Collections.Generic;
using System.Threading.Tasks;
using GameService.Core.Models;

namespace GameService.Core.Interfaces
{
    public interface IGameRepository
    {
        Task<Game> GetByIdAsync(string id);
        Task<IEnumerable<Game>> GetAllAsync();
        Task<IEnumerable<Game>> GetAllAsync(int page = 1, int pageSize = 10);
        Task<IEnumerable<Game>> GetByAuthorIdAsync(string authorId, int page = 1, int pageSize = 10);
        Task<IEnumerable<Game>> GetByFilterIdAsync(string filterId, int page = 1, int pageSize = 10);
        Task<IEnumerable<Game>> SearchByNameAsync(string searchTerm);
        Task<int> GetTotalCountAsync();
        Task<int> GetTotalCountByAuthorIdAsync(string authorId);
        Task<int> GetTotalCountByFilterIdAsync(string filterId);
        Task<Game> AddAsync(Game game);
        Task<Game> UpdateAsync(Game game);
        Task DeleteAsync(Game game);
        Task<IEnumerable<Game>> GetByAuthorIdAsync(string authorId);
    }
} 