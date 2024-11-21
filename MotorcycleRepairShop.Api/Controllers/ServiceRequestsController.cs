using Microsoft.AspNetCore.Mvc;
using MotorcycleRepairShop.Application.Interfaces.Services;
using MotorcycleRepairShop.Application.Model;
using MotorcycleRepairShop.Domain.Enums;
using MotorcycleRepairShop.Infrastructure.Services;

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

        [HttpGet("pagination")]
        public async Task<ActionResult<TableResponse<ServiceRequestTable>>> GetPagination(
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 5,
            [FromQuery] string keyword = "")
            => Ok(await _serviceRequestService.GetServiceRequestPagination(new TableRequest
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Keyword = keyword
            }));

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceRequestInfoDto>> GetById(int id)
            => Ok(await _serviceRequestService.GetServiceRequestById(id));

        /// <summary>
        /// Tạo một yêu cầu dịch vụ mới khi khách đến trực tiếp.
        /// </summary>
        /// <param name="serviceRequestDto">Đối tượng chứa thông tin của yêu cầu dịch vụ cần tạo.</param>
        /// <returns>ID của yêu cầu dịch vụ mới được tạo.</returns>
        /// <remarks>
        /// Yêu cầu dịch vụ sẽ bao gồm các thông tin cần thiết cho một yêu cầu mới.
        /// </remarks>
        [HttpPost("deirec")]
        public async Task<ActionResult<int>> CreateDeirecRequestService(CreateServiceRequestDto serviceRequestDto)
            => CreatedAtAction(nameof(CreateDeirecRequestService), 
                await _serviceRequestService.CreateDeirecServiceRequest(serviceRequestDto));

        /// <summary>
        /// Tạo một yêu cầu dịch vụ mới khi khách ở xa.
        /// </summary>
        /// <param name="serviceRequestDto">Đối tượng chứa thông tin của yêu cầu dịch vụ cần tạo.</param>
        /// <returns>ID của yêu cầu dịch vụ mới được tạo.</returns>
        /// <remarks>
        /// Yêu cầu dịch vụ sẽ bao gồm các thông tin cần thiết cho một yêu cầu mới.
        /// </remarks>
        [HttpPost("remote")]
        public async Task<ActionResult<int>> CreateRemoteRequestService(CreateServiceRequestDto serviceRequestDto)
            => CreatedAtAction(nameof(CreateRemoteRequestService), 
                await _serviceRequestService.CreateRemoteServiceRequest(serviceRequestDto));

        /// <summary>
        /// Tạo một yêu cầu dịch vụ mới khi yêu cầu cứu hộ.
        /// </summary>
        /// <param name="serviceRequestDto">Đối tượng chứa thông tin của yêu cầu dịch vụ cần tạo.</param>
        /// <returns>ID của yêu cầu dịch vụ mới được tạo.</returns>
        /// <remarks>
        /// Yêu cầu dịch vụ sẽ bao gồm các thông tin cần thiết cho một yêu cầu mới.
        /// </remarks>
        [HttpPost("rescue")]
        public async Task<ActionResult<int>> CreateRescueRequestService(CreateServiceRequestDto serviceRequestDto)
            => CreatedAtAction(nameof(CreateRescueRequestService), 
                await _serviceRequestService.CreateRescueServiceRequest(serviceRequestDto));

        /// <summary>
        /// Chỉnh sửa thông tin khách hàng của yêu cầu dịch vụ.
        /// </summary>
        /// <param name="serviceRequestId">Id của yêu cầu dịch vụ.</param>
        /// <param name="serviceRequestDto">Thông tin cần cập nhật.</param>
        /// <returns>Phản hồi NoContent nếu cập nhật thành công.</returns>
        [HttpPatch("{serviceRequestId}")]
        public async Task<ActionResult> UpdateServiceRequestUserInfo(int serviceRequestId,ServiceRequestUserInfoDto serviceRequestDto)
        {
            await _serviceRequestService.UpdateServiceRequestUserInfoById(serviceRequestId, serviceRequestDto);
            return NoContent();
        }

        #region Status: Update
        /// <summary>
        /// Cập nhật trạng thái của một yêu cầu dịch vụ.
        /// </summary>
        /// <param name="id">ID của yêu cầu dịch vụ cần cập nhật.</param>
        /// <param name="status">Trạng thái mới của yêu cầu dịch vụ.</param>
        /// <returns>Phản hồi NoContent nếu cập nhật thành công.</returns>
        /// <remarks>
        /// Các giá trị trạng thái có thể là:
        /// - 1 = Mới
        /// - 2 = Đang kiểm tra
        /// - 3 = Đang đợi thanh toán
        /// - 4 = Đang xử lý
        /// - 5 = Hoàn thành
        /// - 6 = Hủy
        /// </remarks>
        [HttpPatch("status/{id}")]
        public async Task<ActionResult> UpdateServiceRequestStatus(int id, StatusEnum status)
        {
            await _serviceRequestService.UpdateServiceRequestStatus(id, status);
            return NoContent();
        }
        #endregion

        #region Media: Add - Delete

        [HttpPost("images/{id}")]
        public async Task<ActionResult<IEnumerable<string>>> AddImagesToServiceRequest(int id, IEnumerable<IFormFile> images)
            => CreatedAtAction(nameof(AddImagesToServiceRequest), 
                await _serviceRequestService.AddMediaToServiceRequest(id, images, Domain.Enums.MediaType.Image));

        [HttpPost("videos/{id}")]
        public async Task<ActionResult<IEnumerable<string>>> AddVideosToServiceRequest(int id, IEnumerable<IFormFile> videos)
            => CreatedAtAction(nameof(AddVideosToServiceRequest), 
                await _serviceRequestService.AddMediaToServiceRequest(id, videos, Domain.Enums.MediaType.Video));

        /// <summary>
        /// Xóa ảnh của yêu cầu dịch vụ
        /// </summary>
        /// <param name="id">Id của yêu cầu dịch vụ</param>
        /// <param name="imageUrls">Mảng urls của hình ảnh</param>
        /// <returns></returns>
        [HttpDelete("images/{id}")]
        public async Task<ActionResult> DeleteImageInServiceRequest(int id, IEnumerable<string> imageUrls)
        {
            await _serviceRequestService.DeleteMediaInServiceRequest(id, imageUrls, Domain.Enums.MediaType.Image);
            return NoContent();
        }
        /// <summary>
        /// Xóa video của yêu cầu dịch vụ
        /// </summary>
        /// <param name="id">Id của yêu cầu dịch vụ</param>
        /// <param name="videoUrls">Mảng urls của video</param>
        /// <returns></returns>
        [HttpDelete("videos/{id}")]
        public async Task<ActionResult> DeleteVideosInServiceRequest(int id, IEnumerable<string> videoUrls)
        {
            await _serviceRequestService.DeleteMediaInServiceRequest(id, videoUrls, Domain.Enums.MediaType.Video);
            return NoContent();
        }

        #endregion

        #region ServiceItem: UpSert - Delete
        [HttpPut("services/{id}")]
        public async Task<ActionResult<ServiceRequestItemDto>> UpSertServiceRequestItem(int id, UpSertServiceRequestItemDto serviceRequestItemDto)
            => Ok(await _serviceRequestService.UpSertServiceItemToServiceRequest(id, serviceRequestItemDto));

        [HttpDelete("{serviceRequestId}/services/{serviceId}")]
        public async Task<ActionResult> DeleteServiceItem(int serviceRequestId, int serviceId)
        {
            await _serviceRequestService.DeleteServiceItemToServiceRequest(serviceRequestId, serviceId);
            return NoContent();
        }
        #endregion

        #region ServicePart: UpSert - Delete
        [HttpPut("parts/{id}")]
        public async Task<ActionResult<ServiceRequestPartDto>> UpSertServiceRequestPart(int id, UpSertServiceRequestPartDto serviceRequestPartDto)
            => Ok(await _serviceRequestService.UpSertServicePartToServiceRequest(id, serviceRequestPartDto));

        [HttpDelete("{serviceRequestId}/parts/{partId}")]
        public async Task<ActionResult> DeleteServicePart(int serviceRequestId, int partId)
        {
            await _serviceRequestService.DeleteServicePartInServiceRequest(serviceRequestId, partId);
            return NoContent();
        }
        #endregion
    }
}
