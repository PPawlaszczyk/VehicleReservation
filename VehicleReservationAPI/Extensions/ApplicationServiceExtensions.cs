using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using VehicleReservationAPI.Data;
using VehicleReservationAPI.Data.Repositories;
using VehicleReservationAPI.Interfaces;
using VehicleReservationAPI.Services;

namespace VehicleReservationAPI.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllers();
            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlServer(config.GetConnectionString("DefaultConnection"));

            });
            services.AddEndpointsApiExplorer();
            services.AddCors();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly));

            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo() { Title = "info API", Version = "v1" });
                config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name= "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat="JWT",
                    In = ParameterLocation.Header,
                    Description = "Here enter JWT with bearer format like bearer[space] token"
                });
                config.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]
                        {

                        }
                    }
                });
            });
            services.AddScoped<IMessageProducer, MessageProducer>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IVehiclesRepository, VehiclesRepository>();
            services.AddScoped<IReservationRepository, ReservationsRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddSignalR();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //services.AddHostedService<ReservationNotifier>();
            return services;
        }
    }
}
