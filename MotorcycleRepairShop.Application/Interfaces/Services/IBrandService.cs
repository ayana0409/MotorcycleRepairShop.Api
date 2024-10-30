using MotorcycleRepairShop.Application.Model;

namespace MotorcycleRepairShop.Application.Interfaces.Services
{
    public interface IBrandService
    {
        Task<int> CreateBrand(BrandDto brandDto);
        Task DeleteBrand(int id);
        Task<IEnumerable<BrandTableDto>> GetBrandForDropDownList();
        Task<TableResponse<BrandTableDto>> GetBrandPagination(TableRequest request);
        Task<BrandDto> GetById(int id);
        Task<BrandDto> UpdateBrand(int id, BrandDto brandDto);
    }
}
