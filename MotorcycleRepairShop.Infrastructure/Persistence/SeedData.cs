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
            await SeedStatuses(unitOfWork);
            await SeedProblems(unitOfWork);
            await SeedVehicle(unitOfWork);
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
        private static async Task SeedStatuses(IUnitOfWork unitOfWork)
        {
            var dataSet = unitOfWork.Table<Status>();
            if (!dataSet.Any())
            {
                IEnumerable<Status> addList =
                [
                    new(){ Id = 1, StatusName = "Mới", Description = "Yêu cầu của bạn đã được tạo" },
                    new(){ Id = 2, StatusName = "Đang kiểm tra", Description = "Phương tiện của bạn đang được kiểm tra" },
                    new(){ Id = 3, StatusName = "Chờ thanh toán", Description = "Đang chờ thanh toán" },
                    new(){ Id = 4, StatusName = "Đang xử lý", Description = "Phương tiện của bạn đang được xử lý" },
                    new(){ Id = 5, StatusName = "Hoàn thành", Description = "Phương tiện của bạn đã xử lý xong" },
                    new(){ Id = 6, StatusName = "Hủy", Description = "Yêu cầu của bạn đã được hủy" },
                ];
                await dataSet.AddRangeAsync(addList);
                await unitOfWork.SaveChangeAsync();
            }
        }

        private static async Task SeedProblems(IUnitOfWork unitOfWork)
        {
            var dataSet = unitOfWork.Table<Problem>();
            if (!dataSet.Any())
            {
                IEnumerable<Problem> addList =
                [
                    new(){ Name = "Đề không nổ", Description = "Đề không nổ" },
                    new(){ Name = "Đề và đạp không nổ", Description = "Đề và đạp không nổ máy" },
                    new(){ Name = "Kim tốc độ không chạy", Description = "Kim chỉ tốc độ của đồng hồ không chạy" },
                    new(){ Name = "Sạc không vào", Description = "Sạc không vào pin hoặc rất lâu vào" },
                ];
                await dataSet.AddRangeAsync(addList);
                await unitOfWork.SaveChangeAsync();
            }
        }

        private static async Task SeedVehicle(IUnitOfWork unitOfWork)
        {
            var dataSet = unitOfWork.Table<Vehicle>();
            if (!dataSet.Any())
            {
                IEnumerable<Vehicle> addList = [
                    new(){ Name = "Wave RSX", Version = "Xanh - 2023", BrandId = 1 },
                    new(){ Name = "Exciter", Version = "Limited - 2023", BrandId = 2 },
                    new(){ Name = "WinnerX", Version = "Đen nhám - 2023", BrandId = 1 },
                ];
                await dataSet.AddRangeAsync(addList);
                await unitOfWork.SaveChangeAsync();
            }
        }
    }
}
