using Microsoft.EntityFrameworkCore;
using MovieService.Models;

namespace MovieReviewApp.Data
{
	public class ApplicationDbContext : DbContext
	{
		public DbSet<Movie> Movies { get;}
		public DbSet<ContentType> Categories { get;}

		public DbSet<ContentType> Categorys { get;}


		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<ContentType>().HasData(
				new ContentType { Id = 1, Name = "Action" },
				new ContentType { Id = 2, Name = "Comedy" },
				new ContentType { Id = 3, Name = "Drama" }
			);

			modelBuilder.Entity<ContentType>().HasData(
				new ContentType { Id = 1, Name = "Movie" },
				new ContentType { Id = 2, Name = "Series" },
				new ContentType { Id = 3, Name = "Anime" }
			);
		}
	}
}
