using Microsoft.EntityFrameworkCore;
using MovieReviewApp.Common.Middlewares;
using MovieReviewApp.Common.Repository;
using MovieReviewApp.Data;
using MovieService.Models;
using MovieService.Repository;
using MovieService.Service;

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

            Configure(app);

            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DbContext, ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddJwtAuthentication(configuration);

            services.AddScoped<IMovieService, MovieService.Service.MovieService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IContentTypeService, ContentTypeService>();


            services.AddScoped(typeof(IBaseRepository<ContentType>), typeof(BaseRepository<ContentType>));
            services.AddScoped(typeof(IBaseRepository<Category>), typeof(BaseRepository<Category>));
            services.AddScoped<IMovieRepository, MovieRepository>();

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
