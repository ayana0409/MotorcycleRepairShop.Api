using MotorcycleRepairShop.Domain.Entities;

namespace MotorcycleRepairShop.Application.Interfaces.Repositories
{
    public interface IPartRepository : IBaseRepository<Part>
    {
        Task<IEnumerable<Part>> GetByServiceRequestId(int serviceRequestId);
        Task<(IEnumerable<Part>, int)> GetPanigationAsync(int pageIndex, int pageSize, string keyword);
    }
}
