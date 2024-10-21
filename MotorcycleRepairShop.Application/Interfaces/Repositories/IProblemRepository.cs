using MotorcycleRepairShop.Domain.Entities;

namespace MotorcycleRepairShop.Application.Interfaces.Repositories
{
    public interface IProblemRepository : IBaseRepository<Problem>
    {
        Task<(IEnumerable<Problem>, int)> GetPanigationAsync(int pageIndex, int pageSize, string keyword);
    }
}
