using MassTransit;
using Microsoft.EntityFrameworkCore;
using MovieService.AsyncDataClients.Conusmers;
using MovieService.Data;
using MovieService.HealthCheck;
using MovieService.Middlewares;
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

#if DEBUG
            PrintConfiguration(builder.Configuration);
            CheckDbConnection(app);
#endif

            Configure(app);

            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            ConfigureDatabase(services, configuration);
            ConfigureHealthCheck(services);
            ConfigureMassTransit(services, configuration);
            ConfigureJwtAuthentication(services, configuration);
            ConfigureSwagger(services);

            services.AddScoped<IMovieService, Service.MovieService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IContentTypeService, ContentTypeService>();

            services.AddScoped(typeof(IBaseRepository<ContentType>), typeof(BaseRepository<ContentType>));
            services.AddScoped(typeof(IBaseRepository<Category>), typeof(BaseRepository<Category>));
            services.AddScoped<IMovieRepository, MovieRepository>();

            services.AddControllers();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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
            app.UseCors("AllowSpecificOrigin");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
        }

        private static void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DbContext, ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
        }

        private static void ConfigureHealthCheck(IServiceCollection services)
        {
            services.AddScoped<IDatabaseHealthCheck, DatabaseHealthCheck>();
        }

        private static void ConfigureMassTransit(IServiceCollection services, IConfiguration configuration)
        {
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

        private static void ConfigureJwtAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            services.AddJwtAuthentication(configuration);
        }

        private static void ConfigureSwagger(IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
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
                    throw new Exception("Error. No database connection");
                }
                logger.LogWarning("DB IS CONNECTED");
            }
        }
    }
}
