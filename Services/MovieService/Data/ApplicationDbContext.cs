using Microsoft.EntityFrameworkCore;
using MovieService.Models;

namespace MovieReviewApp.Data
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

			modelBuilder.Entity<ContentType>().HasData(
				new ContentType { Id = Guid.NewGuid(), Name = "Action" },
				new ContentType { Id = Guid.NewGuid(), Name = "Comedy" },
				new ContentType { Id = Guid.NewGuid(), Name = "Drama" }
			);

			modelBuilder.Entity<Category>().HasData(
				new Category { Id = Guid.NewGuid(), Name = "Movie" },
				new Category { Id = Guid.NewGuid(), Name = "Series" },
				new Category { Id = Guid.NewGuid(), Name = "Anime" }
			);
		}

	}
}
