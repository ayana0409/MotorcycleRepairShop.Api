using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MotorcycleRepairShop.Application.Interfaces;
using MotorcycleRepairShop.Domain.Entities;
using MotorcycleRepairShop.Domain.Enums;

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
            await SeedPartInventory(unitOfWork);
            await SeedServiceRequest(unitOfWork);
            await SeedPayment(unitOfWork);
        }

        public static async Task SeedApplicationUser(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await SeedRoles(roleManager);

            var adminUser = new ApplicationUser
            {
                UserName = "1234567890",
                Email = "tttoan@example.com",
                FullName = "Trần Thanh Toàn",
                MobilePhone = "1234567890",
                Address = "123 Admin St",
                IsActive = true
            };

            var managerUser = new ApplicationUser
            {
                UserName = "0987654321",
                Email = "ddthuan@example.com",
                FullName = "Dương Đoàn Thuận",
                MobilePhone = "0987654321",
                Address = "123 Admin St",
                IsActive = true
            };

            if (await userManager.FindByNameAsync(adminUser.UserName) == null)
            {
                var result = await userManager.CreateAsync(adminUser, "AdminPassword123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
                Console.WriteLine("Seeding Admin succeeded");
            }

            if (await userManager.FindByNameAsync(managerUser.UserName) == null)
            {
                var result = await userManager.CreateAsync(managerUser, "AdminPassword123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(managerUser, "Manager");
                }
                Console.WriteLine("Seeding Manager succeeded");
            }
        }

        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            var roles = new List<string> { "Admin", "Manager", "Employee" };

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
                    new(){ Name = "Kiểm tra tư vấn", Price = 0 },
                    new(){ Name = "Vá lốp", Price = 15000 },
                    new(){ Name = "Tăng xích", Price = 20000 },
                    new(){ Name = "Thay nhớt", Price = 20000 },
                    new(){ Name = "Thay bugi", Price = 15000 },
                    new(){ Name = "Thay má phanh", Price = 30000 },
                    new(){ Name = "Thay dây curoa", Price = 40000 },
                    new(){ Name = "Sửa chữa bộ đề", Price = 50000 },
                    new(){ Name = "Thay lọc gió", Price = 20000 },
                    new(){ Name = "Thay dây ga", Price = 25000 },
                    new(){ Name = "Sửa điện xe máy", Price = 100000 },
                    new(){ Name = "Thay vòng bi bánh xe", Price = 30000 },
                    new(){ Name = "Rửa xe máy", Price = 30000 },
                    new(){ Name = "Thay bình ắc quy xe máy điện", Price = 50000 },
                    new(){ Name = "Kiểm tra và căn chỉnh phanh đĩa", Price = 40000 }
                };
                await services.AddRangeAsync(serviceAddList);
                await unitOfWork.SaveChangeAsync();
                Console.WriteLine("Seeding Services succeeded");
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
                    new(){ Name = "Suzuki", Country = "Nhật Bản" },
                    new(){ Name = "SYM", Country = "Đài Loan" },
                    new(){ Name = "Piaggio", Country = "Ý" },
                    new(){ Name = "Kawasaki", Country = "Nhật Bản" },
                    new(){ Name = "Kymco", Country = "Đài Loan" },
                    new(){ Name = "Ducati", Country = "Ý" },
                    new(){ Name = "BMW Motorrad", Country = "Đức" },
                    new(){ Name = "VinFast", Country = "Việt Nam" },
                    new(){ Name = "Harley-Davidson", Country = "Mỹ" },
                    new(){ Name = "Pirelli", Country = "Ý" }, // Lốp xe
                    new(){ Name = "Michelin", Country = "Pháp" }, // Lốp xe
                    new(){ Name = "NGK", Country = "Nhật Bản" }, // Bugi
                    new(){ Name = "Motul", Country = "Pháp" }, // Dầu nhớt
                    new(){ Name = "Castrol", Country = "Anh" }, // Dầu nhớt
                    new(){ Name = "Bando", Country = "Nhật Bản" }, // Dây curoa
                    new(){ Name = "DID", Country = "Nhật Bản" }, // Xích
                    new(){ Name = "Nissin", Country = "Nhật Bản" }, // Phanh đĩa
                    new(){ Name = "Bosch", Country = "Đức" }, // Phụ tùng điện tử
                    new(){ Name = "Continental", Country = "Đức" } // Lốp xe
                };
                await dataSet.AddRangeAsync(addList);
                await unitOfWork.SaveChangeAsync();
                Console.WriteLine("Seeding Brands succeeded");
            }
        }

        private static async Task SeedParts(IUnitOfWork unitOfWork)
        {
            var dataSet = unitOfWork.Table<Part>();
            if (!dataSet.Any())
            {
                IEnumerable<Part> addList = new List<Part>
                {
                    new(){ Name = "Lọc gió Wave RSX", WarrantyPeriod = 3, Price = 75000, BrandId = 1 }, // Honda
                    new(){ Name = "Ốp sườn NVX", WarrantyPeriod = 1, Price = 335000, BrandId = 2 }, // Yamaha
                    new(){ Name = "Lọc nhớt Wave Alpha", WarrantyPeriod = 6, Price = 45000, BrandId = 1 }, // Honda
                    new(){ Name = "Dây curoa Vision", WarrantyPeriod = 12, Price = 180000, BrandId = 1 }, // Honda
                    new(){ Name = "Má phanh Sirius", WarrantyPeriod = 6, Price = 90000, BrandId = 2 }, // Yamaha
                    new(){ Name = "Lọc gió Raider 150", WarrantyPeriod = 3, Price = 85000, BrandId = 3 }, // Suzuki
                    new(){ Name = "Cùm phanh trước SH", WarrantyPeriod = 12, Price = 350000, BrandId = 1 }, // Honda
                    new(){ Name = "Lốp xe Vespa LX", WarrantyPeriod = 24, Price = 850000, BrandId = 5 }, // Piaggio
                    new(){ Name = "Bugi NGK CR7HSA", WarrantyPeriod = 12, Price = 55000, BrandId = 13 }, // NGK
                    new(){ Name = "Dầu nhớt Motul 7100", WarrantyPeriod = 0, Price = 190000, BrandId = 15 }, // Motul
                    new(){ Name = "Nhông sên đĩa Exciter 150", WarrantyPeriod = 6, Price = 480000, BrandId = 2 }, // Yamaha
                    new(){ Name = "Lốp xe Michelin Pilot Street 2", WarrantyPeriod = 24, Price = 1200000, BrandId = 14 }, // Michelin
                    new(){ Name = "Dây curoa Bando Lead", WarrantyPeriod = 12, Price = 250000, BrandId = 17 } // Bando
                };

                await dataSet.AddRangeAsync(addList);
                await unitOfWork.SaveChangeAsync();
                Console.WriteLine("Seeding Parts succeeded");
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
                Console.WriteLine("Seeding Statuses succeeded");
            }
        }

        private static async Task SeedProblems(IUnitOfWork unitOfWork)
        {
            var dataSet = unitOfWork.Table<Problem>();
            if (!dataSet.Any())
            {
                IEnumerable<Problem> addList = new List<Problem>
                {
                    new(){ Name = "Đề không nổ", Description = "Đề không nổ, có thể do hỏng bugi, ắc quy yếu hoặc hệ thống điện." },
                    new(){ Name = "Đề và đạp không nổ", Description = "Cả đề và đạp không nổ máy, nguyên nhân có thể do hết xăng, hỏng hệ thống đánh lửa hoặc nén khí không đủ." },
                    new(){ Name = "Kim tốc độ không chạy", Description = "Kim chỉ tốc độ của đồng hồ không chạy, có thể do hỏng dây đồng hồ hoặc cảm biến tốc độ." },
                    new(){ Name = "Sạc không vào", Description = "Sạc không vào pin hoặc rất lâu vào, nguyên nhân có thể là do lỗi ắc quy hoặc hỏng bộ sạc." },
                    new(){ Name = "Xe bị rung lắc", Description = "Xe bị rung lắc khi di chuyển, thường do lốp mòn hoặc vành xe bị cong." },
                    new(){ Name = "Phanh không ăn", Description = "Phanh bóp nhưng không ăn, có thể do má phanh mòn hoặc thiếu dầu phanh." },
                    new(){ Name = "Đèn pha không sáng", Description = "Đèn pha không sáng, nguyên nhân có thể là bóng đèn cháy hoặc hỏng dây dẫn điện." },
                    new(){ Name = "Xe chết máy giữa chừng", Description = "Xe đang chạy bị chết máy, thường do hệ thống xăng gió hoặc điện bị lỗi." },
                    new(){ Name = "Xe không bốc", Description = "Xe chạy yếu, không bốc, có thể do bộ truyền động hoặc động cơ bị lỗi." },
                    new(){ Name = "Tiếng kêu lạ từ động cơ", Description = "Có tiếng kêu lạ từ động cơ, thường do xích cam, bộ truyền động hoặc bạc đạn bị mòn." },
                    new(){ Name = "Đèn báo động cơ sáng", Description = "Đèn báo lỗi động cơ trên bảng đồng hồ sáng, có thể do hệ thống cảm biến hoặc động cơ gặp sự cố." },
                    new(){ Name = "Xe bị hao xăng", Description = "Xe tiêu thụ xăng nhiều hơn bình thường, nguyên nhân có thể do bộ chế hòa khí hoặc hệ thống phun xăng." },
                    new(){ Name = "Xe điện không khởi động", Description = "Xe điện không khởi động, có thể do lỗi pin hoặc mạch điều khiển." },
                    new(){ Name = "Ắc quy nhanh hết điện", Description = "Ắc quy không giữ được điện lâu, nguyên nhân thường do pin đã cũ hoặc bị chai." },
                    new(){ Name = "Lốp xe bị xì hơi", Description = "Lốp xe bị xì hơi, có thể do thủng lốp hoặc van bị rò." }
                };

                await dataSet.AddRangeAsync(addList);
                await unitOfWork.SaveChangeAsync();
                Console.WriteLine("Seeding Problems succeeded");
            }
        }

        private static async Task SeedVehicle(IUnitOfWork unitOfWork)
        {
            var dataSet = unitOfWork.Table<Vehicle>();
            if (!dataSet.Any())
            {
                IEnumerable<Vehicle> addList = new List<Vehicle>
                {
                    new(){ Name = "Wave RSX", Version = "Xanh - 2023", BrandId = 1 }, // Honda
                    new(){ Name = "Exciter", Version = "Limited - 2023", BrandId = 2 }, // Yamaha
                    new(){ Name = "WinnerX", Version = "Đen nhám - 2023", BrandId = 1 }, // Honda
                    new(){ Name = "Lead", Version = "Trắng - 2023", BrandId = 1 }, // Honda
                    new(){ Name = "Sirius", Version = "Đỏ - 2023", BrandId = 2 }, // Yamaha
                    new(){ Name = "NVX", Version = "Xanh đen - 2023", BrandId = 2 }, // Yamaha
                    new(){ Name = "Raider 150", Version = "Xanh GP - 2023", BrandId = 3 }, // Suzuki
                    new(){ Name = "SH Mode", Version = "Đỏ đô - 2023", BrandId = 1 }, // Honda
                    new(){ Name = "Vespa Primavera", Version = "Be - 2023", BrandId = 5 }, // Piaggio
                    new(){ Name = "Vision", Version = "Đen nhám - 2023", BrandId = 1 }, // Honda
                    new(){ Name = "Janus", Version = "Hồng - 2023", BrandId = 2 }, // Yamaha
                    new(){ Name = "VinFast Klara", Version = "Trắng - 2023", BrandId = 10 }, // VinFast
                    new(){ Name = "VinFast Feliz", Version = "Xanh ngọc - 2023", BrandId = 10 }, // VinFast
                    new(){ Name = "Vario 160", Version = "Xám - 2023", BrandId = 1 }, // Honda
                    new(){ Name = "SH 350i", Version = "Đỏ nhám - 2023", BrandId = 1 }, // Honda
                    new(){ Name = "Grande", Version = "Trắng ngọc trai - 2023", BrandId = 2 }, // Yamaha
                    new(){ Name = "GSX-R150", Version = "Xanh GP - 2023", BrandId = 3 } // Suzuki
                };

                await dataSet.AddRangeAsync(addList);
                await unitOfWork.SaveChangeAsync();
                Console.WriteLine("Seeding Vehicle succeeded");
            }
        }

        private static async Task SeedPartInventory(IUnitOfWork unitOfWork)
        {
            var dataSet = unitOfWork.Table<PartInventory>();
            if (!dataSet.Any())
            {
                IEnumerable<PartInventory> addList = new List<PartInventory>
                {
                    new()
                    {
                        BatchNumber = "WRSX001", Supplier = "Honda Việt Nam", Tax = 10, EntryDate = DateTime.UtcNow.AddDays(-10),
                        QuantityReceived = 100, QuantityInStock = 100, EntryPrice = 65000,
                        ProductionDate = DateTime.UtcNow.AddMonths(-3), ExpirationDate = DateTime.UtcNow.AddMonths(21),
                        PartId = 1 // Lọc gió Wave RSX
                    },
                    new()
                    {
                        BatchNumber = "NVX002", Supplier = "Yamaha Motor", Tax = 12, EntryDate = DateTime.UtcNow.AddDays(-5),
                        QuantityReceived = 50, QuantityInStock = 45, EntryPrice = 310000,
                        ProductionDate = DateTime.UtcNow.AddMonths(-2), ExpirationDate = DateTime.UtcNow.AddMonths(22),
                        PartId = 2 // Ốp sườn NVX
                    },
                    new()
                    {
                        BatchNumber = "WAVE003", Supplier = "Honda Việt Nam", Tax = 10, EntryDate = DateTime.UtcNow.AddDays(-15),
                        QuantityReceived = 200, QuantityInStock = 180, EntryPrice = 40000,
                        ProductionDate = DateTime.UtcNow.AddMonths(-4), ExpirationDate = DateTime.UtcNow.AddMonths(20),
                        PartId = 3 // Lọc nhớt Wave Alpha
                    },
                    new()
                    {
                        BatchNumber = "VISION004", Supplier = "Honda Việt Nam", Tax = 10, EntryDate = DateTime.UtcNow.AddDays(-7),
                        QuantityReceived = 150, QuantityInStock = 150, EntryPrice = 160000,
                        ProductionDate = DateTime.UtcNow.AddMonths(-1), ExpirationDate = DateTime.UtcNow.AddMonths(23),
                        PartId = 4 // Dây curoa Vision
                    },
                    new()
                    {
                        BatchNumber = "SIR005", Supplier = "Yamaha Motor", Tax = 12, EntryDate = DateTime.UtcNow.AddDays(-3),
                        QuantityReceived = 120, QuantityInStock = 100, EntryPrice = 80000,
                        ProductionDate = DateTime.UtcNow.AddMonths(-2), ExpirationDate = DateTime.UtcNow.AddMonths(22),
                        PartId = 5 // Má phanh Sirius
                    },
                    new()
                    {
                        BatchNumber = "RAID006", Supplier = "Suzuki Việt Nam", Tax = 10, EntryDate = DateTime.UtcNow.AddDays(-20),
                        QuantityReceived = 80, QuantityInStock = 75, EntryPrice = 80000,
                        ProductionDate = DateTime.UtcNow.AddMonths(-6), ExpirationDate = DateTime.UtcNow.AddMonths(18),
                        PartId = 6 // Lọc gió Raider 150
                    },
                    new()
                    {
                        BatchNumber = "SH007", Supplier = "Honda Việt Nam", Tax = 15, EntryDate = DateTime.UtcNow.AddDays(-30),
                        QuantityReceived = 50, QuantityInStock = 40, EntryPrice = 330000,
                        ProductionDate = DateTime.UtcNow.AddMonths(-5), ExpirationDate = DateTime.UtcNow.AddMonths(25),
                        PartId = 7 // Cùm phanh trước SH
                    },
                    new()
                    {
                        BatchNumber = "VESPA008", Supplier = "Piaggio Việt Nam", Tax = 8, EntryDate = DateTime.UtcNow.AddDays(-25),
                        QuantityReceived = 30, QuantityInStock = 25, EntryPrice = 800000,
                        ProductionDate = DateTime.UtcNow.AddMonths(-4), ExpirationDate = DateTime.UtcNow.AddMonths(26),
                        PartId = 8 // Lốp xe Vespa LX
                    }
                };
                await dataSet.AddRangeAsync(addList);
                await unitOfWork.SaveChangeAsync();
                Console.WriteLine("Seeding Part Inventories succeeded");
            }
        }

        private static async Task SeedServiceRequest(IUnitOfWork unitOfWork)
        {
            var dataSet = unitOfWork.Table<ServiceRequest>();
            if (!dataSet.Any())
            {
                IEnumerable<ServiceRequest> addList = new List<ServiceRequest>
                {
                    new()
                    {
                        MobilePhone = "0123456789",
                        FullName = "Nguyễn Văn A",
                        Address = "123 Đường ABC, Quận 1, TP.HCM",
                        Email = "nguyenvana@gmail.com",
                        IssueDescription = "Xe không khởi động được, cần kiểm tra và sửa chữa",
                        DateSubmitted = DateTime.UtcNow.AddDays(-5),
                        CompletionDate = DateTime.UtcNow.AddDays(-3),
                        StatusId = (int)StatusEnum.UnderInspection,
                        Services = new List<ServiceRequestItem>
                        {
                            new() { ServiceId = 1, Quantity = 1, Price = 0 }, // Kiểm tra tư vấn
                            new() { ServiceId = 2, Quantity = 1, Price = 30000 } // Vá lốp
                        },
                        Parts = new List<ServiceRequestPart>
                        {
                            new() { PartId = 1, Quantity = 1, Price = 75000, WarrantyTo = DateTime.UtcNow.AddMonths(3) } // Lọc gió Wave RSX
                        },
                        Problems = new List<ServiceRequestProblem>
                        {
                            new() { ProblemId = 1 } // Đề không nổ
                        }
                    },
                    new()
                    {
                        MobilePhone = "0987654321",
                        FullName = "Trần Thị B",
                        Address = "456 Đường XYZ, Quận 3, TP.HCM",
                        Email = "tranthib@gmail.com",
                        IssueDescription = "Xe mất điện khi đang chạy",
                        DateSubmitted = DateTime.UtcNow.AddDays(-2),
                        CompletionDate = DateTime.UtcNow.AddDays(-1),
                        StatusId = (int)StatusEnum.Processing,
                        Services = new List<ServiceRequestItem>
                        {
                            new() { ServiceId = 3, Quantity = 1, Price = 7000 }, // Tăng xích
                            new() { ServiceId = 4, Quantity = 1, Price = 50000 } // Thay nhớt
                        },
                        Parts = new List<ServiceRequestPart>
                        {
                            new() { PartId = 3, Quantity = 1, Price = 45000, WarrantyTo = DateTime.UtcNow.AddMonths(6) } // Lọc nhớt Wave Alpha
                        },
                        Problems = new List<ServiceRequestProblem>
                        {
                            new() { ProblemId = 2 } // Đề và đạp không nổ
                        }
                    },
                    new()
                    {
                        MobilePhone = "0934567890",
                        FullName = "Lê Minh C",
                        Address = "789 Đường DEF, Quận 7, TP.HCM",
                        Email = "leminhc@gmail.com",
                        IssueDescription = "Xe không báo tốc độ, cần thay linh kiện",
                        DateSubmitted = DateTime.UtcNow.AddDays(-1),
                        CompletionDate = DateTime.UtcNow,
                        StatusId = (int)StatusEnum.AwaitingPayment,
                        Services = new List<ServiceRequestItem>
                        {
                            new() { ServiceId = 5, Quantity = 1, Price = 100000 } // Sửa đồng hồ tốc độ
                        },
                        Parts = new List<ServiceRequestPart>
                        {
                            new() { PartId = 5, Quantity = 1, Price = 80000, WarrantyTo = DateTime.UtcNow.AddMonths(12) } // Má phanh Sirius
                        },
                        Problems = new List<ServiceRequestProblem>
                        {
                            new() { ProblemId = 3 } // Kim tốc độ không chạy
                        }
                    }
                };

                await dataSet.AddRangeAsync(addList);
                await unitOfWork.SaveChangeAsync();
                Console.WriteLine("Seeding Service Requests succeeded");
            }
        }

        private static async Task SeedPayment(IUnitOfWork unitOfWork)
        {
            var dataSet = unitOfWork.Table<Payment>();
            if (!dataSet.Any())
            {
                IEnumerable<Payment> addList = new List<Payment>
                {
                    new()
                    {
                        Amount = 95000,
                        PaymentDate = DateTime.UtcNow.AddDays(-1),
                        TransactionId = "TXN001",
                        PaymentMethod = PaymentMethodEnum.Cash,
                        IsSuccessful = true,
                        Note = "Thanh toán tiền mặt tại cửa hàng",
                        ServiceRequestId = 2
                    },
                    new()
                    {
                        Amount = 180000,
                        PaymentDate = DateTime.UtcNow,
                        TransactionId = Guid.NewGuid().ToString(),
                        PaymentMethod = PaymentMethodEnum.PayPal,
                        IsSuccessful = true,
                        Note = "Thanh toán qua PayPal",
                        ServiceRequestId = 3
                    }
                };
                await dataSet.AddRangeAsync(addList);
                await unitOfWork.SaveChangeAsync();
                Console.WriteLine("Seeding Payments succeeded");
            }
        }
    }
}
