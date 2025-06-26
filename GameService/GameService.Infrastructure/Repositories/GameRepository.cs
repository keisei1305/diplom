using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameService.Core.Interfaces;
using GameService.Core.Models;
using GameService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GameService.Infrastructure.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly GameDbContext _context;

        public GameRepository(GameDbContext context)
        {
            _context = context;
        }

        public async Task<Game> GetByIdAsync(string id)
        {
            return await _context.Games
                .Include(g => g.Author)
                .Include(g => g.GameFile)
                .Include(g => g.Filters)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<IEnumerable<Game>> GetAllAsync(int page = 1, int pageSize = 10)
        {
            return await _context.Games
                .Include(g => g.Author)
                .Include(g => g.GameFile)
                .Include(g => g.Filters)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Game>> GetByAuthorIdAsync(string authorId, int page = 1, int pageSize = 10)
        {
            return await _context.Games
                .Include(g => g.Author)
                .Include(g => g.GameFile)
                .Include(g => g.Filters)
                .Where(g => g.AuthorId == authorId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Game>> GetByFilterIdAsync(string filterId, int page = 1, int pageSize = 10)
        {
            return await _context.Games
                .Include(g => g.Author)
                .Include(g => g.GameFile)
                .Include(g => g.Filters)
                .Where(g => g.Filters.Any(f => f.Id == filterId))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Games.CountAsync();
        }

        public async Task<int> GetTotalCountByAuthorIdAsync(string authorId)
        {
            return await _context.Games.CountAsync(g => g.AuthorId == authorId);
        }

        public async Task<int> GetTotalCountByFilterIdAsync(string filterId)
        {
            return await _context.Games.CountAsync(g => g.Filters.Any(f => f.Id == filterId));
        }

        public async Task<Game> AddAsync(Game game)
        {
            await _context.Games.AddAsync(game);
            await _context.SaveChangesAsync();
            return game;
        }

        public async Task<Game> UpdateAsync(Game game)
        {
            _context.Games.Update(game);
            await _context.SaveChangesAsync();
            return game;
        }

        public async Task DeleteAsync(Game game)
        {
            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Game>> GetAllAsync()
        {
            return await _context.Games
                .Include(f => f.Author)
                .Include(f => f.Filters)
                .ToListAsync();
        }

        public async Task<IEnumerable<Game>> SearchByNameAsync(string searchTerm)
        {
            return await _context.Games
                .Include(g => g.Author)
                .Include(g => g.GameFile)
                .Include(g => g.Filters)
                .Where(g => g.Name.ToLower().Contains(searchTerm.ToLower()))
                .ToListAsync();
        }

        public async Task<IEnumerable<Game>> GetByAuthorIdAsync(string authorId)
        {
            return await _context.Games
                .Include(g => g.Filters)
                .Where(g => g.AuthorId == authorId)
                .OrderByDescending(g => g.CreatedAt)
                .ToListAsync();
        }
    }
} 