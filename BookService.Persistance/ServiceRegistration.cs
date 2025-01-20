using BookService.Infrastructure.Persistence.Contexts;
using BookService.Infrastructure.Persistence.UnitOfWorks.Custom;
using BookService.Persistance.UnitOfWorks.Custom;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BookService.Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration, bool useInMemoryDatabase)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            if (useInMemoryDatabase)
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase(nameof(ApplicationDbContext)));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
            }          
            services.RegisterUnitOfWorks();

            return services;
        }
        private static void RegisterUnitOfWorks(this IServiceCollection services)
        {
            services.AddScoped<IUserUnitOfWork, UserUnitOfWork>();
            services.AddScoped<IOrderUnitOfWork, OrderUnitOfWork>();
            services.AddScoped<IBookUnitOfWork, BookUnitOfWork>();
        }
    }
}
