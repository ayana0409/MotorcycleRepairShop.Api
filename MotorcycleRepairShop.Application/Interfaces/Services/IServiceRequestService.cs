using MotorcycleRepairShop.Application.Model;

namespace MotorcycleRepairShop.Application.Interfaces.Services
{
    public interface IServiceRequestService
    {
        Task<int> CreateDeirecServiceRequest(ServiceRequestDto serviceRequestDto);
        Task<ServiceRequestDto> GetServiceRequestById(int id);
    }
}
