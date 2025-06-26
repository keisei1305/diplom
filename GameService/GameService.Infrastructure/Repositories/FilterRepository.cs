using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameService.Core.Interfaces;
using GameService.Core.Models;
using GameService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GameService.Infrastructure.Repositories
{
    public class FilterRepository : IFilterRepository
    {
        private readonly GameDbContext _context;

        public FilterRepository(GameDbContext context)
        {
            _context = context;
        }

        public async Task<Filter> GetByIdAsync(string id)
        {
            return await _context.Filters
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<IEnumerable<Filter>> GetAllAsync()
        {
            return await _context.Filters
                .Include(f => f.Games)
                .ToListAsync();
        }

        public async Task<IEnumerable<Filter>> GetByTypeAsync(string filterType)
        {
            return await _context.Filters
                .Include(f => f.Games)
                .Where(f => f.FilterType == filterType)
                .ToListAsync();
        }

        public async Task<Filter> AddAsync(Filter filter)
        {
            await _context.Filters.AddAsync(filter);
            await _context.SaveChangesAsync();
            return filter;
        }

        public async Task<Filter> UpdateAsync(Filter filter)
        {
            _context.Filters.Update(filter);
            await _context.SaveChangesAsync();
            return filter;
        }

        public async Task DeleteAsync(Filter filter)
        {
            _context.Filters.Remove(filter);
            await _context.SaveChangesAsync();
        }
    }
} 