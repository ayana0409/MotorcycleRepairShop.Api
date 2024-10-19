using Microsoft.Extensions.DependencyInjection;
using MotorcycleRepairShop.Application.Interfaces;
using MotorcycleRepairShop.Application.Interfaces.Repositories;
using MotorcycleRepairShop.Application.Interfaces.Services;
using MotorcycleRepairShop.Domain.Entities;
using MotorcycleRepairShop.Infrastructure.Repositories;
using MotorcycleRepairShop.Infrastructure.Services;

namespace MotorcycleRepairShop.Infrastructure
{
    public static class Configurations
    {
        public static void AddDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<ICloudinaryService<Vehicle>, CloudinaryService<Vehicle>>();

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IAuthService, AuthService>();

            services.AddTransient<IServiceRepository, ServiceRepository>();
            services.AddTransient<IBrandRepository, BrandRepository>();
            services.AddTransient<IImageRepository, ImageRepository>();

            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IServiceService, ServiceService>();
            services.AddTransient<IBrandService, BrandService>();
            services.AddTransient<IVehicleService, VehicleService>();
        }
    }
}
