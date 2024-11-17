using MotorcycleRepairShop.Domain.Entities;

namespace MotorcycleRepairShop.Application.Interfaces.Repositories
{
    public interface IVehicleRepository : IBaseRepository<Vehicle>
    {
        Task<Vehicle?> GetById(int id);
        Task<(IEnumerable<Vehicle>, int)> GetPanigationAsync(int pageIndex, int pageSize, string keyword);
        Task<IEnumerable<Vehicle>> GetVehiclesByBrandId(int brandId);
    }
}
