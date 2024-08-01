
using Microsoft.EntityFrameworkCore;
using MovieReviewApp.Common.Repository;
using MovieReviewApp.Data;
using MovieService.Models;
using MovieService.Repository;

namespace MovieReviewApp
{
	public class Program
	{

		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Configuration.AddUserSecrets<Program>();

			ConfigureServices(builder.Services, builder.Configuration);

			var app = builder.Build();

			ConfigureMiddleware(app);
			ConfigureEndpoints(app);

			app.Run();
		}

		private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("DefaultConnection");
			services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

			// Add services to the container.
			services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();

			services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

			services.AddScoped(typeof(IBaseRepository<ContentType>), typeof(BaseRepository<ContentType>));
			services.AddScoped(typeof(IBaseRepository<Category>), typeof(BaseRepository<Category>));
			services.AddScoped<IMovieRepository, MovieRepository>();
		}

		private static void ConfigureMiddleware(WebApplication app)
		{
			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();
		}

		private static void ConfigureEndpoints(WebApplication app)
		{
			app.MapControllers();
		}

	}
}
