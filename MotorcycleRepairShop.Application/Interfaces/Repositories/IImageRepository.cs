using MotorcycleRepairShop.Domain.Entities;

namespace MotorcycleRepairShop.Application.Interfaces.Repositories
{
    public interface IImageRepository
    {
        Task<IEnumerable<Image>> GetByVehicleIdAsync(int id);
    }
}
