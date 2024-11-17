using MotorcycleRepairShop.Application.Model;
using MotorcycleRepairShop.Application.Model.Service;

namespace MotorcycleRepairShop.Application.Interfaces.Services
{
    public interface IHomeService
    {
        Task<IEnumerable<BrandHomeDto>> GetBrandList();
        Task<IEnumerable<PartHomeDto>> GetPartList();
        Task<IEnumerable<ServiceHomeDto>> GetServiceList();
        Task<VehicleHomeDto> GetVehicleById(int id);
        Task<IEnumerable<VehicleHomeDto>> GetVehiclesByBrandId(int brandId);
    }
}
