using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MotorcycleRepairShop.Domain.Entities;

namespace MotorcycleRepairShop.Infrastructure.Persistence
{
    public static class SeedData
    {
        public static void Initialize(this WebApplication webApplication)
        {
            using var scope = webApplication.Services.CreateScope();
            var dbContext = scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

            dbContext.Database.MigrateAsync().GetAwaiter().GetResult();

            SeedApplicationUser(scope.ServiceProvider);

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
                FullName = "Admin User",
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
                    roleManager.CreateAsync(new IdentityRole(role)).Wait();
                }
            }
        }

    }
}
