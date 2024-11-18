﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MotorcycleRepairShop.Infrastructure.Persistence;

#nullable disable

namespace MotorcycleRepairShop.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241118092615_addEmailPropToServiceRequest")]
    partial class addEmailPropToServiceRequest
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("RoleId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ApplicationUserId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRole", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserTokens", (string)null);
                });

            modelBuilder.Entity("MotorcycleRepairShop.Domain.Entities.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime");

                    b.Property<string>("MobilePhone")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("varchar(15)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("ApplicationUser", (string)null);
                });

            modelBuilder.Entity("MotorcycleRepairShop.Domain.Entities.Brand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.HasKey("Id");

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("MotorcycleRepairShop.Domain.Entities.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("ServiceRequestId")
                        .HasColumnType("int");

                    b.Property<int?>("VehicleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ServiceRequestId");

                    b.HasIndex("VehicleId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("MotorcycleRepairShop.Domain.Entities.Part", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("BrandId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("WarrantyPeriod")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BrandId");

                    b.ToTable("Parts");
                });

            modelBuilder.Entity("MotorcycleRepairShop.Domain.Entities.PartInventory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("BatchNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("EntryDate")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal>("EntryPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("PartId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ProductionDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("QuantityInStock")
                        .HasColumnType("int");

                    b.Property<int>("QuantityReceived")
                        .HasColumnType("int");

                    b.Property<string>("Supplier")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.Property<decimal>("Tax")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("PartId");

                    b.ToTable("Inventories");
                });

            modelBuilder.Entity("MotorcycleRepairShop.Domain.Entities.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("IsSuccessful")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Note")
                        .HasColumnType("varchar(500)");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("PaymentMethod")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<int>("ServiceRequestId")
                        .HasColumnType("int");

                    b.Property<string>("TransactionId")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("ServiceRequestId");

                    b.ToTable("Payment");
                });

            modelBuilder.Entity("MotorcycleRepairShop.Domain.Entities.Problem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.HasKey("Id");

                    b.ToTable("Problems");
                });

            modelBuilder.Entity("MotorcycleRepairShop.Domain.Entities.Service", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("MotorcycleRepairShop.Domain.Entities.ServiceRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<DateTime>("CompletionDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("CustomerId")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("DateSubmitted")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.Property<string>("IssueDescripton")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("MobilePhone")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("varchar(11)");

                    b.Property<string>("ServiceType")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("StatusId")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("StatusId");

                    b.ToTable("ServiceRequest");
                });

            modelBuilder.Entity("MotorcycleRepairShop.Domain.Entities.ServiceRequestItem", b =>
                {
                    b.Property<int>("ServiceId")
                        .HasColumnType("int");

                    b.Property<int>("ServiceRequestId")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("ServiceId", "ServiceRequestId");

                    b.HasIndex("ServiceRequestId");

                    b.ToTable("ServiceRequestItems");
                });

            modelBuilder.Entity("MotorcycleRepairShop.Domain.Entities.ServiceRequestPart", b =>
                {
                    b.Property<int>("PartId")
                        .HasColumnType("int");

                    b.Property<int>("ServiceRequestId")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<DateTime?>("WarrantyTo")
                        .HasColumnType("datetime(6)");

                    b.HasKey("PartId", "ServiceRequestId");

                    b.HasIndex("ServiceRequestId");

                    b.ToTable("ServiceRequestPart");
                });

            modelBuilder.Entity("MotorcycleRepairShop.Domain.Entities.ServiceRequestProblem", b =>
                {
                    b.Property<int>("ServiceRequestId")
                        .HasColumnType("int");

                    b.Property<int>("ProblemId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReportedDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("ServiceRequestId", "ProblemId");

                    b.HasIndex("ProblemId");

                    b.ToTable("ServiceRequestProblems");
                });

            modelBuilder.Entity("MotorcycleRepairShop.Domain.Entities.Status", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<string>("StatusName")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.HasKey("Id");

                    b.ToTable("Statuses");
                });

            modelBuilder.Entity("MotorcycleRepairShop.Domain.Entities.Vehicle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("BrandId")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.HasKey("Id");

                    b.HasIndex("BrandId");

                    b.ToTable("Vehicles");
                });

            modelBuilder.Entity("MotorcycleRepairShop.Domain.Entities.Video", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<int>("ServiceRequestId")
                        .HasColumnType("int");

                    b.Property<bool>("SubmittedByStaff")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.HasIndex("ServiceRequestId");

                    b.ToTable("Videos");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("MotorcycleRepairShop.Domain.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("MotorcycleRepairShop.Domain.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("MotorcycleRepairShop.Domain.Entities.ApplicationUser", null)
                        .WithMany("UserRoles")
                        .HasForeignKey("ApplicationUserId");

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MotorcycleRepairShop.Domain.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("MotorcycleRepairShop.Domain.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MotorcycleRepairShop.Domain.Entities.Image", b =>
                {
                    b.HasOne("MotorcycleRepairShop.Domain.Entities.ServiceRequest", "ServiceRequest")
                        .WithMany("Images")
                        .HasForeignKey("ServiceRequestId");

                    b.HasOne("MotorcycleRepairShop.Domain.Entities.Vehicle", "Vehicle")
                        .WithMany("Images")
                        .HasForeignKey("VehicleId");

                    b.Navigation("ServiceRequest");

                    b.Navigation("Vehicle");
                });

            modelBuilder.Entity("MotorcycleRepairShop.Domain.Entities.Part", b =>
                {
                    b.HasOne("MotorcycleRepairShop.Domain.Entities.Brand", "Brand")
                        .WithMany()
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Brand");
                });

            modelBuilder.Entity("MotorcycleRepairShop.Domain.Entities.PartInventory", b =>
                {
                    b.HasOne("MotorcycleRepairShop.Domain.Entities.Part", "Part")
                        .WithMany("Inventories")
                        .HasForeignKey("PartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Part");
                });

            modelBuilder.Entity("MotorcycleRepairShop.Domain.Entities.Payment", b =>
                {
                    b.HasOne("MotorcycleRepairShop.Domain.Entities.ServiceRequest", "ServiceRequest")
                        .WithMany("Payments")
                        .HasForeignKey("ServiceRequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ServiceRequest");
                });

            modelBuilder.Entity("MotorcycleRepairShop.Domain.Entities.ServiceRequest", b =>
                {
                    b.HasOne("MotorcycleRepairShop.Domain.Entities.ApplicationUser", "Customer")
                        .WithMany("ServiceRequests")
                        .HasForeignKey("CustomerId");

                    b.HasOne("MotorcycleRepairShop.Domain.Entities.Status", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("MotorcycleRepairShop.Domain.Entities.ServiceRequestItem", b =>
                {
                    b.HasOne("MotorcycleRepairShop.Domain.Entities.Service", "Service")
                        .WithMany("Requests")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MotorcycleRepairShop.Domain.Entities.ServiceRequest", "ServiceRequest")
                        .WithMany("Services")
                        .HasForeignKey("ServiceRequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Service");

                    b.Navigation("ServiceRequest");
                });

            modelBuilder.Entity("MotorcycleRepairShop.Domain.Entities.ServiceRequestPart", b =>
                {
                    b.HasOne("MotorcycleRepairShop.Domain.Entities.Part", "Part")
                        .WithMany("ServiceRequests")
                        .HasForeignKey("PartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MotorcycleRepairShop.Domain.Entities.ServiceRequest", "ServiceRequest")
                        .WithMany("Parts")
                        .HasForeignKey("ServiceRequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Part");

                    b.Navigation("ServiceRequest");
                });

            modelBuilder.Entity("MotorcycleRepairShop.Domain.Entities.ServiceRequestProblem", b =>
                {
                    b.HasOne("MotorcycleRepairShop.Domain.Entities.Problem", "Problem")
                        .WithMany("ServiceRequestProblems")
                        .HasForeignKey("ProblemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MotorcycleRepairShop.Domain.Entities.ServiceRequest", "ServiceRequest")
                        .WithMany("Problems")
                        .HasForeignKey("ServiceRequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Problem");

                    b.Navigation("ServiceRequest");
                });

            modelBuilder.Entity("MotorcycleRepairShop.Domain.Entities.Vehicle", b =>
                {
                    b.HasOne("MotorcycleRepairShop.Domain.Entities.Brand", "Brand")
                        .WithMany()
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Brand");
                });

            modelBuilder.Entity("MotorcycleRepairShop.Domain.Entities.Video", b =>
                {
                    b.HasOne("MotorcycleRepairShop.Domain.Entities.ServiceRequest", "ServiceRequest")
                        .WithMany("Videos")
                        .HasForeignKey("ServiceRequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ServiceRequest");
                });

            modelBuilder.Entity("MotorcycleRepairShop.Domain.Entities.ApplicationUser", b =>
                {
                    b.Navigation("ServiceRequests");

                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("MotorcycleRepairShop.Domain.Entities.Part", b =>
                {
                    b.Navigation("Inventories");

                    b.Navigation("ServiceRequests");
                });

            modelBuilder.Entity("MotorcycleRepairShop.Domain.Entities.Problem", b =>
                {
                    b.Navigation("ServiceRequestProblems");
                });

            modelBuilder.Entity("MotorcycleRepairShop.Domain.Entities.Service", b =>
                {
                    b.Navigation("Requests");
                });

            modelBuilder.Entity("MotorcycleRepairShop.Domain.Entities.ServiceRequest", b =>
                {
                    b.Navigation("Images");

                    b.Navigation("Parts");

                    b.Navigation("Payments");

                    b.Navigation("Problems");

                    b.Navigation("Services");

                    b.Navigation("Videos");
                });

            modelBuilder.Entity("MotorcycleRepairShop.Domain.Entities.Vehicle", b =>
                {
                    b.Navigation("Images");
                });
#pragma warning restore 612, 618
        }
    }
}
