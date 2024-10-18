using MotorcycleRepairShop.Domain.Entities;

namespace MotorcycleRepairShop.Application.Interfaces.Repositories
{
    public interface IServiceRepository
    {
        Task<(IEnumerable<Service>, int)> GetPanigationAsync(int pageIndex, int pageSize, string keyword);
    }
}
