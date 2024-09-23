using Microsoft.EntityFrameworkCore;
using ReviewService.Models;

namespace ReviewService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Review> Reviews { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure index on MovieId
            modelBuilder.Entity<Review>()
                .HasIndex(r => r.MovieId);

            // Configure index on  UserId
            modelBuilder.Entity<Review>()
                .HasIndex(r => r.UserId);
        }
    }
}
