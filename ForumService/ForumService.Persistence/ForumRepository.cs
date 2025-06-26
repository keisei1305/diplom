using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ForumService.Core.Entities;
using ForumService.Core.Interfaces;

namespace ForumService.Persistence
{
    public class ForumRepository : IForumRepository
    {
        private readonly ForumDbContext _context;
        public ForumRepository(ForumDbContext context) => _context = context;

        public async Task<IEnumerable<Forum>> GetAllAsync() => await _context.Forums.ToListAsync();
        public async Task<Forum?> GetByIdAsync(int id) => await _context.Forums.FindAsync(id);
        public async Task AddAsync(Forum forum)
        {
            _context.Forums.Add(forum);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Forum forum)
        {
            _context.Forums.Update(forum);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var forum = await _context.Forums.FindAsync(id);
            if (forum != null)
            {
                _context.Forums.Remove(forum);
                await _context.SaveChangesAsync();
            }
        }
    }
} 