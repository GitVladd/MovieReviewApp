
using MassTransit;
using Microsoft.EntityFrameworkCore;
using MovieReviewApp.Common.Middlewares;
using ReviewService.AsyncDataClients;
using ReviewService.Data;
using ReviewService.Dtos;

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

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DbContext, ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddJwtAuthentication(configuration);

            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

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

            services.AddTransient<MovieRequestClient>();
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
