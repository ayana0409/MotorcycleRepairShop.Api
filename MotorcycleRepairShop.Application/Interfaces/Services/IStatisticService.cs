using MotorcycleRepairShop.Application.Model;

namespace MotorcycleRepairShop.Application.Interfaces.Services
{
    public interface IStatisticService
    {
        Task<IEnumerable<ServiceRequestStatisticsDto>> GetServiceRequestStatisticsAsync(DateTime startDate, DateTime? endDate);
    }
}
