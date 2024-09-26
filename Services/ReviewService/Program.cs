using MassTransit;
using Microsoft.EntityFrameworkCore;
using ReviewService.AsyncDataClients;
using ReviewService.Data;
using ReviewService.Dtos;
using ReviewService.Middlewares;
using ReviewService.Models;
using ReviewService.Repository;
using ReviewService.Service;

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
            ConfigureDbContext(services, configuration);
            ConfigureMassTransit(services, configuration);
            ConfigureJwtAuthentication(services, configuration);

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddScoped<IReviewService, ReviewService.Service.ReviewService>();

            services.AddScoped(typeof(IBaseRepository<Review>), typeof(BaseRepository<Review>));

            services.AddTransient<MovieRequestClient>();

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

        private static void ConfigureDbContext(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DbContext, ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
        }

        private static void ConfigureMassTransit(IServiceCollection services, IConfiguration configuration)
        {
            var rabbitMQOptions = configuration.GetSection("RabbitMQ");

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMQOptions["Host"], ushort.Parse(rabbitMQOptions["Port"]), "/", h =>
                    {
                        h.Username(rabbitMQOptions["Username"]);
                        h.Password(rabbitMQOptions["Password"]);
                    });
                    cfg.ConfigureEndpoints(context);
                });

                x.AddRequestClient<MovieExistsRequestDto>();
            });
        }

        private static void ConfigureJwtAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            services.AddJwtAuthentication(configuration);
        }
    }
}
