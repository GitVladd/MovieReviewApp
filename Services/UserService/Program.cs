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
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Configuration.AddUserSecrets<Program>();
			builder.Configuration.AddJsonFile("identityRules.json", optional: false, reloadOnChange: false);

			ConfigureServices(builder.Services, builder.Configuration);

			var app = builder.Build();

			//using (var scope = app.Services.CreateScope())
			//{
			//	var services = scope.ServiceProvider;
			//	await InitializeRoles(services);
			//}

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

			services.Configure<IdentityOptions>(options =>
			{
				// Password settings
				var passwordOptions = configuration.GetSection("PasswordRules");
				options.Password.RequireDigit = passwordOptions.GetValue<bool>("RequireDigit");
				options.Password.RequireLowercase = passwordOptions.GetValue<bool>("RequireLowercase");
				options.Password.RequireUppercase = passwordOptions.GetValue<bool>("RequireUppercase");
				options.Password.RequireNonAlphanumeric = passwordOptions.GetValue<bool>("RequireNonAlphanumeric");
				options.Password.RequiredLength = passwordOptions.GetValue<int>("RequiredLength");
				options.Password.RequiredUniqueChars = passwordOptions.GetValue<int>("RequiredUniqueChars");

				// Lockout settings
				var lockoutOptions = configuration.GetSection("LockoutRules");
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(lockoutOptions.GetValue<int>("DefaultLockoutTimeSpanInMinutes"));
				options.Lockout.MaxFailedAccessAttempts = lockoutOptions.GetValue<int>("MaxFailedAccessAttempts");
				options.Lockout.AllowedForNewUsers = lockoutOptions.GetValue<bool>("AllowedForNewUsers");

				// User settings
				var userOptions = configuration.GetSection("UserRules");
				options.User.AllowedUserNameCharacters = userOptions.GetValue<string>("AllowedUserNameCharacters");
				options.User.RequireUniqueEmail = userOptions.GetValue<bool>("RequireUniqueEmail");
			});


			services.AddControllers();

			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();

			services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

			services.AddJwtAuthentication(configuration);
			
			services.AddScoped<IUserService, UserService.Service.UserService>();
			//services.AddScoped(typeof(IBaseRepository<User>), typeof(BaseRepository<User>));

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


		public static async Task InitializeRoles(IServiceProvider serviceProvider)
		{
			var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

			var roles = new[] { "Admin", "Moderator", "User" };

			foreach (var role in roles)
			{
				if (!await roleManager.RoleExistsAsync(role))
				{
					await roleManager.CreateAsync(new IdentityRole<Guid>(role));
				}
			}
		}
	}
}
