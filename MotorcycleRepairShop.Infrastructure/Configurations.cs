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
            services.AddScoped<ICloudinaryService<ServiceRequest>, CloudinaryService<ServiceRequest>>();

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IAuthService, AuthService>();

            services.AddTransient<IServiceRepository, ServiceRepository>();
            services.AddTransient<IBrandRepository, BrandRepository>();
            services.AddTransient<IImageRepository, ImageRepository>();
            services.AddTransient<IVehicleRepository, VehicleRepository>();
            services.AddTransient<IPartRepository, PartRepository>();
            services.AddTransient<IProblemRepository, ProblemRepository>();
            services.AddTransient<IPartInventoryRepository, PartInventoryRepository>();
            services.AddTransient<IVideoRepository, VideoRepository>();
            services.AddTransient<IServiceRequestRepository, ServiceRequestRepository>();

            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IServiceService, ServiceService>();
            services.AddTransient<IBrandService, BrandService>();
            services.AddTransient<IVehicleService, VehicleService>();
            services.AddTransient<IPartService, PartService>();
            services.AddTransient<IProblemService, ProblemService>();
            services.AddTransient<IPartInventoryService, PartInventoryService>();
            services.AddTransient<IServiceRequestService, ServiceRequestService>();
        }
    }
}
