using MassTransit;
using Microsoft.EntityFrameworkCore;
using MovieReviewApp.Common.Middlewares;
using MovieReviewApp.Common.Repository;
using MovieService.AsyncDataClients.Conusmers;
using MovieService.Data;
using MovieService.Dtos;
using MovieService.HealthCheck;
using MovieService.Models;
using MovieService.Repository;
using MovieService.Service;

namespace MovieService
{
    public class Program
    {

        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddUserSecrets<Program>();


            ConfigureServices(builder.Services, builder.Configuration);

            var app = builder.Build();

            //#if DEBUG
            PrintConfiguration(builder.Configuration);
            CheckDbConnection(app);
            //#endif

            Configure(app);

            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DbContext, ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            services.AddScoped<IDatabaseHealthCheck, DatabaseHealthCheck>();

            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddJwtAuthentication(configuration);

            services.AddScoped<IMovieService, Service.MovieService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IContentTypeService, ContentTypeService>();


            services.AddScoped(typeof(IBaseRepository<ContentType>), typeof(BaseRepository<ContentType>));
            services.AddScoped(typeof(IBaseRepository<Category>), typeof(BaseRepository<Category>));
            services.AddScoped<IMovieRepository, MovieRepository>();

            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

            var rabbitMQOptions = configuration.GetSection("RabbitMQ");

            services.AddMassTransit(x =>
            {
                x.AddConsumer<MovieExistsConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMQOptions["Host"], ushort.Parse(rabbitMQOptions["Port"]), "/", h =>
                    {
                        h.Username(rabbitMQOptions["Username"]);
                        h.Password(rabbitMQOptions["Password"]);
                    });

                    cfg.ReceiveEndpoint("movie_exists_queue", e =>
                    {
                        e.ConfigureConsumer<MovieExistsConsumer>(context);
                    });
                });
            });

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

        private static void PrintConfiguration(IConfiguration configuration)
        {
            foreach (var section in configuration.GetChildren())
            {
                PrintSection(section, "");
            }
        }

        private static void PrintSection(IConfigurationSection section, string indent)
        {
            Console.WriteLine($"{indent}{section.Key} = {section.Value}");

            foreach (var child in section.GetChildren())
            {
                PrintSection(child, indent + "  ");
            }
        }

        private static void CheckDbConnection(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                var dbHealthCheck = scope.ServiceProvider.GetRequiredService<IDatabaseHealthCheck>();
                var canConnect = dbHealthCheck.CheckConnectionAsync().Result;

                if (!canConnect)
                {
                    logger.LogError("Error. No database connection");
                }
                logger.LogWarning("DB IS CONNECTED");
            }
        }
    }
}
