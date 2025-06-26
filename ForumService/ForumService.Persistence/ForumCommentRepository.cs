using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ForumService.Core.Entities;
using ForumService.Core.Interfaces;

namespace ForumService.Persistence
{
    public class ForumCommentRepository : IForumCommentRepository
    {
        private readonly ForumDbContext _context;
        public ForumCommentRepository(ForumDbContext context) => _context = context;

        public async Task<IEnumerable<ForumComment>> GetByForumIdAsync(int forumId)
            => await _context.ForumComments.Where(c => c.ForumId == forumId).ToListAsync();

        public async Task<ForumComment?> GetByIdAsync(int id)
            => await _context.ForumComments.FindAsync(id);

        public async Task AddAsync(ForumComment comment)
        {
            _context.ForumComments.Add(comment);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var comment = await _context.ForumComments.FindAsync(id);
            if (comment != null)
            {
                _context.ForumComments.Remove(comment);
                await _context.SaveChangesAsync();
            }
        }
    }
} 