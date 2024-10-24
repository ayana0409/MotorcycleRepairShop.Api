using MotorcycleRepairShop.Domain.Entities;

namespace MotorcycleRepairShop.Application.Interfaces.Repositories
{
    public interface IServiceRepository : IBaseRepository<Service>
    {
        Task<IEnumerable<Service>> GetByServiceRequestId(int serviceRequestId);
        Task<(IEnumerable<Service>, int)> GetPanigationAsync(int pageIndex, int pageSize, string keyword);
    }
}
