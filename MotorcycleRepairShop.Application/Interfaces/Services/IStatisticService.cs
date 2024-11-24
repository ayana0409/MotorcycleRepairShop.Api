using MotorcycleRepairShop.Application.Model;

namespace MotorcycleRepairShop.Application.Interfaces.Services
{
    public interface IStatisticService
    {
        Task<StatisticDto> GetStatisticsAsync(DateTime startDate, DateTime? endDate);
        Task<IEnumerable<ServiceRequestStatisticsDto>> GetServiceRequestStatisticsAsync(DateTime startDate, DateTime? endDate);
    }
}
