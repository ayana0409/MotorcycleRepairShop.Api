using MotorcycleRepairShop.Application.Model;

namespace MotorcycleRepairShop.Application.Interfaces.Services
{
    public interface IPartInventoryService
    {
        Task<List<int>> CreatePartInventories(List<PartInventoryDto> partInventoryDtos);
        Task<int> CreatePartInventory(PartInventoryDto partInventoryDto);
        Task<IEnumerable<PartInventoryDto>> GetAvailableInventoriesByPartId(int partId);
    }
}
