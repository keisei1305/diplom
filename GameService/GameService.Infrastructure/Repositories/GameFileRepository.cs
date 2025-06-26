using System.Threading.Tasks;
using GameService.Core.Interfaces;
using GameService.Core.Models;
using GameService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GameService.Infrastructure.Repositories
{
    public class GameFileRepository : IGameFileRepository
    {
        private readonly GameDbContext _context;

        public GameFileRepository(GameDbContext context)
        {
            _context = context;
        }

        public async Task<GameFile> GetByIdAsync(string id)
        {
            return await _context.GameFiles
                .Include(gf => gf.Game)
                .FirstOrDefaultAsync(gf => gf.Id == id);
        }

        public async Task<GameFile> GetByGameIdAsync(string gameId)
        {
            return await _context.GameFiles
                .Include(gf => gf.Game)
                .FirstOrDefaultAsync(gf => gf.Game.Id == gameId);
        }

        public async Task<GameFile> AddAsync(GameFile gameFile)
        {
            await _context.GameFiles.AddAsync(gameFile);
            await _context.SaveChangesAsync();
            return gameFile;
        }

        public async Task<GameFile> UpdateAsync(GameFile gameFile)
        {
            _context.GameFiles.Update(gameFile);
            await _context.SaveChangesAsync();
            return gameFile;
        }

        public async Task DeleteAsync(GameFile gameFile)
        {
            _context.GameFiles.Remove(gameFile);
            await _context.SaveChangesAsync();
        }
    }
} 