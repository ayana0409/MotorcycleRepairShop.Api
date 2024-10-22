using MotorcycleRepairShop.Application.Interfaces.Repositories;
using MotorcycleRepairShop.Domain.Entities;
using MotorcycleRepairShop.Infrastructure.Persistence;
 
namespace MotorcycleRepairShop.Infrastructure.Repositories
{
    public class PartInventoryRepository : BaseRepository<PartInventory>, IPartInventoryRepository
    {
        public PartInventoryRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
