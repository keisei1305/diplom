using System.Collections.Generic;
using System.Threading.Tasks;
using GameService.Core.Interfaces;
using GameService.Core.Models;
using GameService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GameService.Infrastructure.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly GameDbContext _context;

        public AuthorRepository(GameDbContext context)
        {
            _context = context;
        }

        public async Task<Author> GetByIdAsync(string id)
        {
            return await _context.Authors
                .Include(a => a.Games)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Author> GetByUserIdAsync(string userId)
        {
            return await _context.Authors
                .Include(a => a.Games)
                .FirstOrDefaultAsync(a => a.UserId == userId);
        }

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            return await _context.Authors
                .Include(a => a.Games)
                .ToListAsync();
        }

        public async Task<Author> AddAsync(Author author)
        {
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();
            return author;
        }

        public async Task<Author> UpdateAsync(Author author)
        {
            _context.Authors.Update(author);
            await _context.SaveChangesAsync();
            return author;
        }

        public async Task DeleteAsync(Author author)
        {
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Author>> GetAllAsync(int page, int pagecount)
        {
            return await _context.Authors
                .Include(a => a.Games)
                .Skip((page - 1) * pagecount)
                .Take(pagecount)
                .ToListAsync();
        }
    }
} 