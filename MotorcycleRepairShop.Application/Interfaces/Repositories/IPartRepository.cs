using MotorcycleRepairShop.Domain.Entities;

namespace MotorcycleRepairShop.Application.Interfaces.Repositories
{
    public interface IPartRepository
    {
        Task<(IEnumerable<Part>, int)> GetPanigationAsync(int pageIndex, int pageSize, string keyword);
    }
}
