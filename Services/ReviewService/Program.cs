
using MovieReviewApp.Common.Middlewares;

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
			services.AddControllers();

			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();

			services.AddJwtAuthentication(configuration);

            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();
        }

		private static void Configure(WebApplication app)
		{
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}
            app.UseExceptionHandler();

			app.UseHttpsRedirection();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllers();


        }
    }
}
