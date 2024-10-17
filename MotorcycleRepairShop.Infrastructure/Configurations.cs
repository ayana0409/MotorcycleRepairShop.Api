using Microsoft.Extensions.DependencyInjection;
using MotorcycleRepairShop.Application.Interfaces;
using MotorcycleRepairShop.Application.Interfaces.Services;
using MotorcycleRepairShop.Infrastructure.Repositories;
using MotorcycleRepairShop.Infrastructure.Services;

namespace MotorcycleRepairShop.Infrastructure
{
    public static class Configurations
    {
        public static void AddDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IAuthService, AuthService>();
        }
    }
}
