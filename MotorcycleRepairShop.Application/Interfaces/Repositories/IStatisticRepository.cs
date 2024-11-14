using MotorcycleRepairShop.Application.Model;

namespace MotorcycleRepairShop.Application.Interfaces.Repositories
{
    public interface IStatisticRepository
    {
        Task<IEnumerable<ServiceRequestStatisticsDto>> GetServiceRequestStatisticByDayAsync(DateTime startDay, DateTime endDate);
    }
}
