using MotorcycleRepairShop.Domain.Entities;

namespace MotorcycleRepairShop.Application.Interfaces.Repositories
{
    public interface IVideoRepository : IBaseRepository<Video>
    {
        Task<IEnumerable<Video>> GetByRequestIdAsync(int id);
    }
}
