using MotorcycleRepairShop.Application.Model;

namespace MotorcycleRepairShop.Application.Interfaces.Repositories
{
    public interface IStatisticRepository
    {
        Task<decimal> GetCostStatisticByDay(DateTime startDay, DateTime endDate);
        Task<decimal> GetRevenueStatisticByDay(DateTime startDay, DateTime endDate);
        Task<IEnumerable<ServiceRequestStatisticsDto>> GetServiceRequestStatisticByDay(DateTime startDay, DateTime endDate);
        Task<decimal> GetTaxCostStatisticByDay(DateTime startDay, DateTime endDate);
    }
}
