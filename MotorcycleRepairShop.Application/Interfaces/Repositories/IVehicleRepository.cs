using MotorcycleRepairShop.Domain.Entities;

namespace MotorcycleRepairShop.Application.Interfaces.Repositories
{
    public interface IVehicleRepository
    {
        Task<(IEnumerable<Vehicle>, int)> GetPanigationAsync(int pageIndex, int pageSize, string keyword);
    }
}
