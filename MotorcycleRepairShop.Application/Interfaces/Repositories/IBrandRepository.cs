using MotorcycleRepairShop.Domain.Entities;

namespace MotorcycleRepairShop.Application.Interfaces.Repositories
{
    public interface IBrandRepository : IBaseRepository<Brand>
    {
        Task<(IEnumerable<Brand>, int)> GetPanigationAsync(int pageIndex, int pageSize, string keyword);
    }
}
