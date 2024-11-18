using MotorcycleRepairShop.Application.Model;

namespace MotorcycleRepairShop.Application.Interfaces.Services
{
    public interface IStatisticService
    {
        Task<decimal> GetRevenueStatisticsAsync(DateTime startDate, DateTime? endDate);
        Task<IEnumerable<ServiceRequestStatisticsDto>> GetServiceRequestStatisticsAsync(DateTime startDate, DateTime? endDate);
    }
}
