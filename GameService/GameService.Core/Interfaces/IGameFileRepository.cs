using System.Threading.Tasks;
using GameService.Core.Models;

namespace GameService.Core.Interfaces
{
    public interface IGameFileRepository
    {
        Task<GameFile> GetByIdAsync(string id);
        Task<GameFile> GetByGameIdAsync(string gameId);
        Task<GameFile> AddAsync(GameFile gameFile);
        Task<GameFile> UpdateAsync(GameFile gameFile);
        Task DeleteAsync(GameFile gameFile);
    }
} 