using MotorcycleRepairShop.Domain.Entities;

namespace MotorcycleRepairShop.Application.Interfaces.Repositories
{
    public interface IImageRepository
    {
        void Delete(Image image);
        Task<IEnumerable<Image>> GetByVehicleIdAsync(int id);
    }
}
