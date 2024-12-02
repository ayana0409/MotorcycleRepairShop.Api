using CloudinaryDotNet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MotorcycleRepairShop.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MotorcycleRepairShop.Infrastructure.Persistence.Configuration
{
    public static class Configurations
    {
        public static void AddConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnectionString")
                    ?? throw new ArgumentNullException("Connection string is not configure.");

            var validIssuer = configuration.GetSection("JWT:ValidIssuer");
            var validAudience = configuration.GetSection("JWT:ValidAudience");

            services.AddDbContext<ApplicationDbContext>(
                options => options.UseMySQL(connectionString));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                   .AddEntityFrameworkStores<ApplicationDbContext>()
                   .AddSignInManager<SignInManager<ApplicationUser>>()
                   .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                options.Lockout.MaxFailedAccessAttempts = 3;
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.UseSecurityTokenValidators = true;
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    //ValidIssuer = configuration["JWT:ValidIssuer"],
                    //ValidAudience = configuration["JWT:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"])),
                    ClockSkew = TimeSpan.Zero,
                    RoleClaimType = ClaimTypes.Role,
                    NameClaimType = ClaimTypes.Name,
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        Console.WriteLine($"Challenge triggered: {context.AuthenticateFailure?.Message}");
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        //var token = context.SecurityToken as JwtSecurityToken;
                        //if (token != null)
                        //{
                        //    Console.WriteLine("Token Validated:");
                        //    Console.WriteLine("Issuer: " + token.Issuer);
                        //    Console.WriteLine("Audience: " + token.Audiences.FirstOrDefault());
                        //    foreach (var claim in token.Claims)
                        //    {
                        //        Console.WriteLine($"Claim: {claim.Type} = {claim.Value}");
                        //    }
                        //}
                       
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddSingleton(s =>
            {
                var cloudName = configuration["Cloudinary:CloudName"];
                var apiKey = configuration["Cloudinary:ApiKey"];
                var apiSecret = configuration["Cloudinary:ApiSecret"];

                return new Cloudinary(new Account(cloudName, apiKey, apiSecret));
            });

            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(@"/home/app/.aspnet/DataProtection-Keys/"))
                .SetApplicationName("MotorcycleRepairShop.Api");
        }
    }
}
