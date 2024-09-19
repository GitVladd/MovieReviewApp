using Microsoft.EntityFrameworkCore;
using MovieService.Models;

namespace MovieService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Movie> Movies => Set<Movie>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<ContentType> ContentTypes => Set<ContentType>();


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique();

            modelBuilder.Entity<ContentType>()
                .HasIndex(ct => ct.Name)
                .IsUnique();


            modelBuilder.Entity<Category>().HasData(
                new Category { Id = Guid.NewGuid(), Name = "Action" },
                new Category { Id = Guid.NewGuid(), Name = "Comedy" },
                new Category { Id = Guid.NewGuid(), Name = "Drama" }
            );

            modelBuilder.Entity<ContentType>().HasData(
                new ContentType { Id = Guid.NewGuid(), Name = "Movie" },
                new ContentType { Id = Guid.NewGuid(), Name = "Series" },
                new ContentType { Id = Guid.NewGuid(), Name = "Anime" }
            );
        }

    }
}
