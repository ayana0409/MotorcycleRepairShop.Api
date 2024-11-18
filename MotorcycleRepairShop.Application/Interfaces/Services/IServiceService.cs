using MotorcycleRepairShop.Application.Model;
using MotorcycleRepairShop.Application.Model.Service;

namespace MotorcycleRepairShop.Application.Interfaces.Services
{
    public interface IServiceService
    {
        Task<int> CreateService(ServiceDto serviceDto);
        Task DeleteService(int id);
        Task<ServiceDto> GetById(int id);
        Task<TableResponse<ServiceTableDto>> GetPagination(TableRequest request);
        Task<IEnumerable<ServiceForDropdownDto>> GetServicesForDropDownList();
        Task<ServiceDto> UpdateService(int id, ServiceDto serviceDto);
    }
}
