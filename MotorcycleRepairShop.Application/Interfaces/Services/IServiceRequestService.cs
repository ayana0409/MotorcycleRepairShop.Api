using Microsoft.AspNetCore.Http;
using MotorcycleRepairShop.Application.Model;

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
        Task<IEnumerable<string>> AddImagesToServiceRequest(int serviceRequestId, IEnumerable<IFormFile> images);
        Task DeleteImagesInServiceRequest(int serviceRequestId, IEnumerable<string> imageUrls);
    }
}
