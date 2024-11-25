using CloudinaryDotNet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MotorcycleRepairShop.Domain.Entities;
using System.Text;

namespace MotorcycleRepairShop.Infrastructure.Persistence.Configuration
{
    public static class Configurations
    {
        public static void AddConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnectionString")
                    ?? throw new ArgumentNullException("Connection string is not configure.");

            var validIssuers = configuration.GetSection("JWT:ValidIssuers").Get<string[]>();
            var validAudiences = configuration.GetSection("JWT:ValidAudiences").Get<string[]>();
            if (validAudiences == null || validAudiences.Length == 0)
            {
                throw new Exception("Valid audiences configuration is empty");
            }

            services.AddDbContext<ApplicationDbContext>(
                options => options.UseMySQL(connectionString));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                   .AddEntityFrameworkStores<ApplicationDbContext>()
                   .AddSignInManager<SignInManager<ApplicationUser>>()
                   .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(option =>
            {
                option.Lockout.AllowedForNewUsers = true;
                option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                option.Lockout.MaxFailedAccessAttempts = 3;
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
                    ValidateLifetime = true,
                    ValidIssuers = validIssuers,
                    ValidAudiences = validAudiences,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JWT:Secret").Value)),
                    ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        //Console.WriteLine($"Token validated: {context.SecurityToken}");
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        Console.WriteLine($"Authentication challenge: {context.AuthenticateFailure?.Message}");
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

            services.AddAuthorization();

            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(@"/home/app/.aspnet/DataProtection-Keys/"))
                .SetApplicationName("MotorcycleRepairShop.Api");
        }
    }
}
