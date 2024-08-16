using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieReviewApp.Common.Repository;
using UserService.Data;
using UserService.Models;
using UserService.Service;

namespace ReviewService
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			ConfigureServices(builder.Services, builder.Configuration);

			var app = builder.Build();

			Configure(app);

			app.Run();
		}

		private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<ApplicationDbContext>(options =>
			  options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

			services.AddIdentity<User, IdentityRole<Guid>>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();

			services.AddControllers();

			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();

			services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

			services.AddJwtAuthentication(configuration);

			services.AddScoped<IUserService, UserService.Service.UserService>();
			services.AddScoped(typeof(IBaseRepository<User>), typeof(BaseRepository<User>));

		}

		private static void Configure(WebApplication app)
		{
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllers();
		}
	}
}
