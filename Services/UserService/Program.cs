using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieReviewApp.Common.Middlewares;
using UserService.Data;
using UserService.Middleware;
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

			Configure(app);

			app.Run();
		}

		private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
		{
            services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = HttpLoggingFields.All;
                logging.RequestHeaders.Add("sec-ch-ua");
                logging.MediaTypeOptions.AddText("application/javascript");
                logging.RequestBodyLogLimit = 4096;
                logging.ResponseBodyLogLimit = 4096;
                logging.CombineLogs = true;
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200")
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });

            services.AddControllers();

			services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();

			services.AddDbContext<ApplicationDbContext>(options =>
			  options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

			services.AddIdentity<User, IdentityRole<Guid>>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();

			ConfigureIdentity(services, configuration);

			services.AddJwtAuthentication(configuration);

			services.AddScoped<IUserService, UserService.Service.UserService>();
            services.AddScoped<IJwtGeneratorService, JwtGeneratorService>();

            services.AddExceptionHandler<UserGlobalExceptionHandler>();
            services.AddExceptionHandler<GlobalExceptionHandler>();
			services.AddProblemDetails();
        }

		private static void Configure(WebApplication app)
		{
            app.UseHttpLogging();
            if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}
            app.UseExceptionHandler();

            app.UseCors("AllowSpecificOrigin");

            app.UseHttpsRedirection();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllers();

        }

		private static void ConfigureIdentity(IServiceCollection services, IConfiguration configuration)
		{
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
        }
	}
}
