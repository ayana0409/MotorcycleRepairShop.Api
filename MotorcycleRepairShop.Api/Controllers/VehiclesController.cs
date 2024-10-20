using Microsoft.AspNetCore.Mvc;
using MotorcycleRepairShop.Application.Interfaces.Services;
using MotorcycleRepairShop.Application.Model;
using MotorcycleRepairShop.Application.Model.Service;
using MotorcycleRepairShop.Domain.Entities;

namespace MotorcycleRepairShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehiclesController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }
        [HttpGet("pagination")]
        public async Task<ActionResult<TableResponse<ServiceTableDto>>> GetPagination(
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 5,
            [FromQuery] string keyword = "")
            => Ok(await _vehicleService.GetVehiclePagination(new TableRequest
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Keyword = keyword
            }));

        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleDto>> GetVehicle(int id)
            => Ok(await _vehicleService.GetVehicleById(id));

        [HttpPost]
        public async Task<ActionResult<int>> CreateVehicle([FromForm] CreateVehicleDto vehicleDto)
            => Ok(await _vehicleService.CreateVehicle(vehicleDto));

        [HttpPut("{id}")]
        public async Task<ActionResult<VehicleDto>> UpdateVehicle(int id, [FromForm] UpdateVehicleDto vehicleDto)
            => Ok(await _vehicleService.UpdateVehicle(id, vehicleDto));

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVehicle(int id)
        {
            await _vehicleService.DeleteVehicle(id);
            return NoContent();
        }
    }
}
