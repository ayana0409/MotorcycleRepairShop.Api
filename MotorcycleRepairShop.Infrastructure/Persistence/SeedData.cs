using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MotorcycleRepairShop.Application.Interfaces;
using MotorcycleRepairShop.Domain.Entities;

namespace MotorcycleRepairShop.Infrastructure.Persistence
{
    public static class SeedData
    {
        public static void Initialize(this WebApplication webApplication)
        {
            using var scope = webApplication.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var dbContext = serviceProvider
                .GetRequiredService<ApplicationDbContext>();

            dbContext.Database.MigrateAsync().GetAwaiter().GetResult();
            SeedApplicationUser(serviceProvider);
            SeedServices(serviceProvider);
        }

        public static void SeedApplicationUser(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            SeedRoles(roleManager).GetAwaiter();

            var adminUser = new ApplicationUser
            {
                UserName = "admin@example.com",
                Email = "admin@example.com",
                FullName = "Tiêu Đế Lỏ",
                MobilePhone = "1234567890",
                Address = "123 Admin St",
                IsActive = true
            };

            if (userManager.FindByEmailAsync(adminUser.Email).Result == null)
            {
                var result = userManager.CreateAsync(adminUser, "AdminPassword123!").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(adminUser, "Admin").Wait();
                }
            }
        }
        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            var roles = new List<string> { "Admin", "Employee", "Customer" };

            foreach (var role in roles)
            {
                if (!roleManager.RoleExistsAsync(role).Result)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private static void SeedServices(IServiceProvider serviceProvider)
        {
            var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();

            var services = unitOfWork.Table<Service>();
            if (!services.Any())
            {
                IEnumerable<Service> serviceAddList = [
                    new(){ Name = "Vá lốp", Price = 5000 },
                    new(){ Name = "Tăng xích", Price = 7000 },
                    new(){ Name = "Thay nhớt", Price = 4000 },
                    ];
                services.AddRangeAsync(serviceAddList).Wait();
                unitOfWork.SaveChangeAsync().Wait();
            }
           
        }

    }
}
