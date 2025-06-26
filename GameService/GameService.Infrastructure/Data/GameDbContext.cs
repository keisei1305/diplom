using Microsoft.EntityFrameworkCore;
using GameService.Core.Models;

namespace GameService.Infrastructure.Data
{
    public class GameDbContext : DbContext
    {
        public GameDbContext(DbContextOptions<GameDbContext> options) : base(options)
        {
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Filter> Filters { get; set; }
        public DbSet<GameFile> GameFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Game>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.AuthorId).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired();

                entity.HasOne(e => e.Author)
                    .WithMany(a => a.Games)
                    .HasForeignKey(e => e.AuthorId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.GameFile)
                    .WithOne(gf => gf.Game)
                    .HasForeignKey<Game>(e => e.GameFileId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.Filters)
                    .WithMany(f => f.Games);
            });

            modelBuilder.Entity<Author>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired();
            });

            modelBuilder.Entity<Filter>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FilterType).IsRequired();
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired();
            });

            modelBuilder.Entity<GameFile>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Weight).IsRequired();
                entity.Property(e => e.Path).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired();
                entity.HasOne(e => e.Game)
                    .WithOne(g => g.GameFile)
                    .HasForeignKey<GameFile>(e => e.GameId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
} 