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

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.Initialize();

    app.UseHttpsRedirection();

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

