using MotorcycleRepairShop.Application.Model;

namespace MotorcycleRepairShop.Application.Interfaces.Services
{
    public interface IPartInventoryService
    {
        Task<List<int>> CreatePartInventories(List<CreatePartInventoryDto> partInventoryDtos);
        Task<int> CreatePartInventory(CreatePartInventoryDto partInventoryDto);
        Task<IEnumerable<PartInventoryDto>> GetAvailableInventoriesByPartId(int partId);
    }
}
