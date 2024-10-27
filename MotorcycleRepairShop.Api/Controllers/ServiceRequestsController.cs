using Microsoft.AspNetCore.Mvc;
using MotorcycleRepairShop.Application.Interfaces.Services;
using MotorcycleRepairShop.Application.Model;

namespace MotorcycleRepairShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceRequestsController : ControllerBase
    {
        private readonly IServiceRequestService _serviceRequestService;

        public ServiceRequestsController(IServiceRequestService serviceRequestService)
        {
            _serviceRequestService = serviceRequestService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CreateServiceRequestDto>> GetById(int id)
            => Ok(await _serviceRequestService.GetServiceRequestById(id));

        [HttpPost("deirec")]
        public async Task<ActionResult<int>> CreateDeirecRequestService(CreateServiceRequestDto serviceRequestDto)
            => Ok(await _serviceRequestService.CreateDeirecServiceRequest(serviceRequestDto));

        [HttpPost("remote")]
        public async Task<ActionResult<int>> CreateRemoteRequestService(CreateServiceRequestDto serviceRequestDto)
            => Ok(await _serviceRequestService.CreateRemoteServiceRequest(serviceRequestDto));

        [HttpPost("rescue")]
        public async Task<ActionResult<int>> CreateRescueRequestService(CreateServiceRequestDto serviceRequestDto)
            => Ok(await _serviceRequestService.CreateRescueServiceRequest(serviceRequestDto));

        [HttpPatch("{serviceRequestId}")]
        public async Task<ActionResult> UpdateServiceRequestUserInfo(int serviceRequestId,ServiceRequestUserInfoDto serviceRequestDto)
        {
            await _serviceRequestService.UpdateServiceRequestUserInfoById(serviceRequestId, serviceRequestDto);
            return NoContent();
        }

        #region Media: Add - Delete

        [HttpPost("images/{id}")]
        public async Task<ActionResult<IEnumerable<string>>> AddImagesToServiceRequest(int id, IEnumerable<IFormFile> images)
            => CreatedAtAction(nameof(AddImagesToServiceRequest), 
                await _serviceRequestService.AddMediaToServiceRequest(id, images, Domain.Enums.MediaType.Image));

        [HttpPost("videos/{id}")]
        public async Task<ActionResult<IEnumerable<string>>> AddVideosToServiceRequest(int id, IEnumerable<IFormFile> videos)
            => CreatedAtAction(nameof(AddVideosToServiceRequest), 
                await _serviceRequestService.AddMediaToServiceRequest(id, videos, Domain.Enums.MediaType.Video));

        [HttpDelete("images/{id}")]
        public async Task<ActionResult> DeleteImageInServiceRequest(int id, IEnumerable<string> imageUrls)
        {
            await _serviceRequestService.DeleteMediaInServiceRequest(id, imageUrls, Domain.Enums.MediaType.Image);
            return NoContent();
        }

        [HttpDelete("videos/{id}")]
        public async Task<ActionResult> DeleteVideosInServiceRequest(int id, IEnumerable<string> videoUrls)
        {
            await _serviceRequestService.DeleteMediaInServiceRequest(id, videoUrls, Domain.Enums.MediaType.Video);
            return NoContent();
        }

        #endregion

        #region ServiceItem: UpSert - Delete
        [HttpPut("service/{id}")]
        public async Task<ActionResult<ServiceRequestItemDto>> UpSertServiceRequestItem(int id, UpsSertServiceRequestItemDto serviceRequestItemDto)
            => Ok(await _serviceRequestService.UpSertServiceItemToServiceRequest(id, serviceRequestItemDto));

        [HttpDelete("{serviceRequestId}/service/{serviceId}")]
        public async Task<ActionResult> DeleteServiceIten(int serviceRequestId, int serviceId)
        {
            await _serviceRequestService.DeleteServiceItemToerviceRequest(serviceRequestId, serviceId);
            return NoContent();
        }
        #endregion
    }
}
