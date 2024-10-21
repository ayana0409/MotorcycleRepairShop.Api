﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MotorcycleRepairShop.Application.Interfaces;
using MotorcycleRepairShop.Domain.Entities;

namespace MotorcycleRepairShop.Infrastructure.Persistence
{
    public static class SeedData
    {
        public static async Task Initialize(this WebApplication webApplication)
        {
            using var scope = webApplication.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var dbContext = serviceProvider
                .GetRequiredService<ApplicationDbContext>();
            await dbContext.Database.MigrateAsync();

            var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
            await SeedApplicationUser(serviceProvider);
            await SeedServices(unitOfWork);
            await SeedBrands(unitOfWork);
            await SeedParts(unitOfWork);
        }

        public static async Task SeedApplicationUser(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await SeedRoles(roleManager);

            var adminUser = new ApplicationUser
            {
                UserName = "admin@example.com",
                Email = "admin@example.com",
                FullName = "Tiêu Đế Lỏ",
                MobilePhone = "1234567890",
                Address = "123 Admin St",
                IsActive = true
            };

            if (await userManager.FindByEmailAsync(adminUser.Email) == null)
            {
                var result = await userManager.CreateAsync(adminUser, "AdminPassword123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }

        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            var roles = new List<string> { "Admin", "Employee", "Customer" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private static async Task SeedServices(IUnitOfWork unitOfWork)
        {
            var services = unitOfWork.Table<Service>();
            if (!services.Any())
            {
                IEnumerable<Service> serviceAddList = new List<Service>
                {
                    new(){ Name = "Vá lốp", Price = 5000 },
                    new(){ Name = "Tăng xích", Price = 7000 },
                    new(){ Name = "Thay nhớt", Price = 4000 },
                };
                await services.AddRangeAsync(serviceAddList);
                await unitOfWork.SaveChangeAsync();
            }
        }

        private static async Task SeedBrands(IUnitOfWork unitOfWork)
        {
            var dataSet = unitOfWork.Table<Brand>();
            if (!dataSet.Any())
            {
                IEnumerable<Brand> addList = new List<Brand>
                {
                    new(){ Name = "Honda", Country = "Nhật Bản" },
                    new(){ Name = "Yamaha", Country = "Nhật Bản" },
                    new(){ Name = "SYM", Country = "Đài Loan" },
                };
                await dataSet.AddRangeAsync(addList);
                await unitOfWork.SaveChangeAsync();
            }
        }

        private static async Task SeedParts(IUnitOfWork unitOfWork)
        {
            var dataSet = unitOfWork.Table<Part>();
            if (!dataSet.Any())
            {
                IEnumerable<Part> addList = new List<Part>
                {
                    new(){ Name = "Lọc gió Wave RSX", WarrantyPeriod = 3, Price = 75000, BrandId = 1 },
                    new(){ Name = "Ốp sườn NVX", WarrantyPeriod = 1, Price = 335000, BrandId = 2 },
                    new(){ Name = "Lọc nhớt Wave Alpha", WarrantyPeriod = 6, Price = 45000, BrandId = 1 },
                };
                await dataSet.AddRangeAsync(addList);
                await unitOfWork.SaveChangeAsync();
            }
        }
    }
}
