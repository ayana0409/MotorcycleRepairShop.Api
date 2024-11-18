using MotorcycleRepairShop.Application.Model;

namespace MotorcycleRepairShop.Application.Interfaces.Repositories
{
    public interface IStatisticRepository
    {
        Task<decimal> GetRevenueStatisticByDay(DateTime startDay, DateTime endDate);
        Task<IEnumerable<ServiceRequestStatisticsDto>> GetServiceRequestStatisticByDay(DateTime startDay, DateTime endDate);
    }
}
