
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using MotorcycleRepairShop.Application.Configurations.Models;
using MotorcycleRepairShop.Domain.Entities;

namespace MotorcycleRepairShop.Application.Configurations
{
    public static class Configurations
    {
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "API Documentation",
                    Version = "v1",
                    Description = "API documentation for frontend developers."
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your token in the text input below."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                var xmlFile = "MotorcycleRepairShop.Api.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\MotorcycleRepairShop.Api\bin\Debug\net8.0", xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        public static void AddVNPayConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<VNPayConfig>(configuration.GetSection("VNPay"));
        }
    }
}
