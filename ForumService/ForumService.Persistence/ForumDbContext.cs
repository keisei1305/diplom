using Microsoft.EntityFrameworkCore;
using ForumService.Core.Entities;

namespace ForumService.Persistence
{
    public class ForumDbContext : DbContext
    {
        public ForumDbContext(DbContextOptions<ForumDbContext> options) : base(options) { }
        public DbSet<Forum> Forums { get; set; }
        public DbSet<ForumComment> ForumComments { get; set; }
    }
} 