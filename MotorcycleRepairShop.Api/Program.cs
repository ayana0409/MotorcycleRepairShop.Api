using Microsoft.AspNetCore.Mvc.ApplicationModels;
using MotorcycleRepairShop.Application.Configurations;
using MotorcycleRepairShop.Infrastructure;
using MotorcycleRepairShop.Infrastructure.Persistence;
using MotorcycleRepairShop.Infrastructure.Persistence.Configuration;
using MotorcycleRepairShop.Share.Configuration;
using Serilog;


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

Log.Information("Starting Motorcycle Repair Shop API up");

try
{
    builder.Host.UseSerilog(Serilogger.Configure);
    builder.Services.Configure<RouteOptions>(options
        => options.LowercaseQueryStrings = true);

    builder.Services.AddConfigureServices(builder.Configuration);
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    builder.Services.AddDependencyInjection();

    builder.Services.AddVNPayConfiguration(builder.Configuration);
    builder.Services.AddSmtpConfiguration(builder.Configuration);

    builder.Services.AddControllers(options =>
    {
        options.Filters.Add<ExceptionFilter>();
        options.Conventions.Add(new RouteTokenTransformerConvention(new LowerCaseParameterTransformer()));
    });
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerConfiguration();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigin",
            builder =>
            {
                builder.WithOrigins("http://localhost:5173", "http://26.139.159.129:5000")
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials();
            });
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseCors("AllowSpecificOrigin");

    app.Initialize().GetAwaiter();

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal)) throw;

    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down Ordering API complete");
    Log.CloseAndFlush();
}

