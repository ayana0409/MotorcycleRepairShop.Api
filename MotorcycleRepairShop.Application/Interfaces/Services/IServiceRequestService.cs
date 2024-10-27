using Microsoft.AspNetCore.Http;
using MotorcycleRepairShop.Application.Model;
using MotorcycleRepairShop.Domain.Enums;

namespace MotorcycleRepairShop.Application.Interfaces.Services
{
    public interface IServiceRequestService
    {
        Task<int> CreateRescueServiceRequest(CreateServiceRequestDto serviceRequestDto);
        Task<int> CreateDeirecServiceRequest(CreateServiceRequestDto serviceRequestDto);
        Task<int> CreateRemoteServiceRequest(CreateServiceRequestDto serviceRequestDto);
        Task<ServiceRequestDto> GetServiceRequestById(int id);
        Task<ServiceRequestItemDto> UpSertServiceItemToServiceRequest(int serviceRequestId, UpsSertServiceRequestItemDto serviceRequestDto);
        Task DeleteServiceItemToerviceRequest(int serviceRequestId, int serviceId);
        Task UpdateServiceRequestUserInfoById(int serviceRequestId, ServiceRequestUserInfoDto serviceRequestUserInfoDto);
        Task<IEnumerable<string>> AddMediaToServiceRequest(int serviceRequestId, IEnumerable<IFormFile> mediaData, MediaType type);
        Task DeleteMediaInServiceRequest(int serviceRequestId, IEnumerable<string> mediaUrls, MediaType type);
    }
}
