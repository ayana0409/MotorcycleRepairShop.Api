using Microsoft.AspNetCore.Mvc;
using MotorcycleRepairShop.Application.Interfaces.Services;
using MotorcycleRepairShop.Application.Model;

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

        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleDto>> GetVehicle(int id)
            => Ok(await _vehicleService.GetVehicleById(id));

        [HttpPost]
        public async Task<ActionResult<int>> CreateVehicle([FromForm] CreateVehicleDto vehicleDto)
            => Ok(await _vehicleService.CreateVehicle(vehicleDto));
    }
}
