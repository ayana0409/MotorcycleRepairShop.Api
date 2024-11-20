using Microsoft.AspNetCore.Mvc;
using MotorcycleRepairShop.Application.Interfaces.Services;
using MotorcycleRepairShop.Application.Model;
using MotorcycleRepairShop.Application.Model.Service;

namespace MotorcycleRepairShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IServiceService _service;

        public ServicesController(IServiceService service) => _service = service;

        [HttpGet("dropdown")]
        public async Task<ActionResult<IEnumerable<ServiceForDropdownDto>>> GetForDropDownList()
            => Ok(await _service.GetServicesForDropDownList());

        [HttpGet("pagination")]
        public async Task<ActionResult<TableResponse<ServiceTableDto>>> GetPagination(
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 5,
            [FromQuery] string keyword = "")
            => Ok(await _service.GetPagination(new TableRequest
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    Keyword = keyword
                }));

        /// <summary>
        /// Lấy thông tin dịch vụ bằng Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceDto>> GetServiceById(int id)
            => Ok(await _service.GetById(id));

        /// <summary>
        /// Tạo dịch vụ
        /// </summary>
        /// <param name="serviceDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> CreateService(ServiceDto serviceDto)
            => CreatedAtAction(nameof(CreateService), await _service.CreateService(serviceDto));

        /// <summary>
        /// Cập nhật thông tin dịch vụ
        /// </summary>
        /// <param name="id"></param>
        /// <param name="serviceDto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceDto>> UpdateService(int id, ServiceDto serviceDto)
            => Ok(await _service.UpdateService(id, serviceDto));

        /// <summary>
        /// Xóa dịch vụ bằng id
        /// </summary>
        /// <param name="id">ID của dịch vụ</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteService(int id){
            await _service.DeleteService(id);
            return NoContent();
        }
    }
}
