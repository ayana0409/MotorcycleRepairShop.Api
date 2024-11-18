using MotorcycleRepairShop.Application.Model;

namespace MotorcycleRepairShop.Application.Interfaces.Services
{
    public interface IPartService
    {
        Task<int> CreatePart(PartDto partDto);
        Task DeletePart(int id);
        Task<IEnumerable<PartForDropdownDto>> GetPartsForDropDownList();
        Task<PartDto> GetPartById(int partId);
        Task<TableResponse<PartTableDto>> GetPartPagination(TableRequest request);
        Task<PartDto> UpdatePart(int id, PartDto partDto);
    }
}
