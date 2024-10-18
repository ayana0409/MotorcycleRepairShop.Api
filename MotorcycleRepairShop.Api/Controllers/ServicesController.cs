using Microsoft.AspNetCore.Http;
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

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceDto>> GetServiceById(int id)
            => Ok(await _service.GetById(id));

        [HttpPost]
        public async Task<ActionResult<int>> CreateService(ServiceDto serviceDto)
            => Ok(await _service.CreateService(serviceDto));

        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceDto>> UpdateService(int id, ServiceDto serviceDto)
            => Ok(await _service.UpdateService(id, serviceDto));

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteService(int id){
            await _service.DeleteService(id);
            return NoContent();
        }
    }
}
