using Serilog;
using System.Runtime.CompilerServices;

namespace MotorcycleRepairShop.Infrastructure.Services
{
    public abstract class BaseService
    {
        protected readonly ILogger _logger;

        public BaseService(ILogger logger)
        {
            _logger = logger;
        }

        protected void LogStart([CallerMemberName] string methodName = "")
        {
            _logger.Information($"START - {methodName}");
        }

        protected void LogStart(int id, [CallerMemberName] string methodName = "")
        {
            _logger.Information($"START - {methodName}: {id}");
        }

        protected void LogEnd([CallerMemberName] string methodName = "")
        {
            _logger.Information($"END - {methodName}");
        }

        protected void LogEnd(int id, [CallerMemberName] string methodName = "")
        {
            _logger.Information($"END - {methodName}: {id}");
        }

        protected void LogSuccess(int id, [CallerMemberName] string methodName = "")
        {
            _logger.Information($"{methodName} successfully: {id}");
        }

        protected void LogSuccess(string data, [CallerMemberName] string methodName = "")
        {
            _logger.Information($"{methodName} successfully: {data}");
        }
    }
}
