using MotorcycleRepairShop.Application.Model;

namespace MotorcycleRepairShop.Application.Interfaces.Services
{
    public interface IServiceRequestService
    {
        Task<int> CreateRescueRescueRequest(CreateServiceRequestDto serviceRequestDto);
        Task<int> CreateDeirecServiceRequest(CreateServiceRequestDto serviceRequestDto);
        Task<int> CreateRemoteServiceRequest(CreateServiceRequestDto serviceRequestDto);
        Task<ServiceRequestDto> GetServiceRequestById(int id);
        Task<ServiceRequestItemDto> UpSertServiceToServiceRequest(int serviceRequestId, UpsSertServiceRequestItemDto serviceRequestDto);
    }
}
