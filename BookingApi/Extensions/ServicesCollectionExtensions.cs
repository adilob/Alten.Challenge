using BookingApi.Application.Interfaces;
using BookingApi.Application.Managers;
using BookingApi.Infrastructure.Data;
using BookingApi.Infrastructure.Data.Context;
using BookingApi.Infrastructure.Interfaces;
using BookingApi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace BookingApi.Extensions
{
    public static class ServicesCollectionExtensions
    {
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "Alten Booking API",
                        Description = "ASP.NET core API to register users and make reservations",
                        Contact = new OpenApiContact
                        {
                            Name = "Adilo E Bertoncello",
                            Email = "adilobertoncello@gmail.com",
                            Url = new Uri("https://www.linkedin.com/in/adilo-eduardo-bertoncello-7aa67a7/")
                        }
                    });

                var path = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
                options.IncludeXmlComments(path);
            });

            services.AddSwaggerGenNewtonsoftSupport();
        }

        public static void AddManagers(this IServiceCollection services)
        {
            services.AddScoped<ICustomerManager, CustomerManager>();
            services.AddScoped<IRoomManager, RoomManager>();
            services.AddScoped<IReservationManager, ReservationManager>();
        }

        public static void AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
        }

        public static void AddDbContext(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseSqlServer(
                    configurationManager["ConnectionStrings:SqlConnection"],
                    x => x.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName));
            });
        }
    }
}
