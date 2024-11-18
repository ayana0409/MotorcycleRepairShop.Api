using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MotorcycleRepairShop.Domain.Entities;
using MotorcycleRepairShop.Domain.Enums;
using System.Reflection.Emit;

namespace MotorcycleRepairShop.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Problem> Problems { get; set; }
        public DbSet<PartInventory> Inventories { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Payment> Payment { get; set; }

        //public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public DbSet<ServiceRequestProblem> ServiceRequestProblems { get; set; }
        public DbSet<ServiceRequestItem> ServiceRequestItems { get; set; }
        public DbSet<ServiceRequestPart> ServiceRequestPart { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().ToTable("ApplicationUser");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRole");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

            builder.Entity<ApplicationUser>()
                .HasIndex(x => x.UserName).IsUnique();
            builder.Entity<ApplicationUser>()
                .HasIndex(x => x.Email).IsUnique();

            builder.Entity<ServiceRequest>()
                .Property(e => e.ServiceType)
                .HasConversion(
                    v => v.ToString(),
                    v => (ServiceType)Enum.Parse(typeof(ServiceType), v));

            builder.Entity<ServiceRequestProblem>()
                .HasKey(srp => new { srp.ServiceRequestId, srp.ProblemId });

            builder.Entity<ServiceRequestProblem>()
                .HasOne(srp => srp.ServiceRequest)
                .WithMany(sr => sr.Problems)
                .HasForeignKey(srp => srp.ServiceRequestId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ServiceRequestProblem>()
                .HasOne(srp => srp.Problem)
                .WithMany(p => p.ServiceRequestProblems)
                .HasForeignKey(srp => srp.ProblemId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ServiceRequestItem>()
                .HasKey(sri => new {sri.ServiceId, sri.ServiceRequestId});

            builder.Entity<ServiceRequestPart>()
                .HasKey(srp => new { srp.PartId, srp.ServiceRequestId });

            builder.Entity<Payment>()
                .Property(p => p.PaymentMethod)
                .HasConversion<string>();
        }

        public override int SaveChanges()
        {
            UpdateTotalPrices();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTotalPrices();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTotalPrices()
        {
            var entries = ChangeTracker.Entries<ServiceRequest>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                var serviceRequest = entry.Entity;
                serviceRequest.TotalPrice = CalculateTotalPrice(serviceRequest);
            }
        }

        private decimal CalculateTotalPrice(ServiceRequest request)
        {
            // Tương tự phương thức ở trên
            var servicePrice = request.Services?.Sum(s => s.Price * s.Quantity) ?? 0;
            var partPrice = request.Parts?.Sum(p => p.Price * p.Quantity) ?? 0;

            return servicePrice + partPrice;
        }
    }
}
