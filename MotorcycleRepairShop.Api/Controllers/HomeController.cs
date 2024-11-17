using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MotorcycleRepairShop.Application.Interfaces.Services;
using MotorcycleRepairShop.Application.Model;
using MotorcycleRepairShop.Application.Model.Service;

namespace MotorcycleRepairShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IHomeService _homeService;

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        [HttpGet("service-list")]
        public async Task<ActionResult<IEnumerable<ServiceHomeDto>>> GetServicelist()
            => Ok(await _homeService.GetServiceList());

        [HttpGet("part-list")]
        public async Task<ActionResult<IEnumerable<PartHomeDto>>> GetPartList()
            => Ok(await _homeService.GetPartList());

        [HttpGet("brand-list")]
        public async Task<ActionResult<IEnumerable<BrandHomeDto>>> GetBrandList()
            => Ok(await _homeService.GetBrandList());

        [HttpGet("vehicle-list-by-brandid")]
        public async Task<ActionResult<IEnumerable<VehicleHomeDto>>> GetVehicleList([FromQuery]int id)
            => Ok(await _homeService.GetVehiclesByBrandId(id));

        [HttpGet("vehicle-info")]
        public async Task<ActionResult<VehicleHomeDto>> GetVehicleById([FromQuery]int id)
            => Ok(await _homeService.GetVehicleById(id));

        [HttpGet("service-requests-by-mobile-phone")]
        public async Task<ActionResult<IEnumerable<ServiceRequestHomeDto>>>
            GetServiceRequestByMobilePhone([FromQuery] string mobilePhone)
            => Ok(await _homeService.GetServiceRequestsByMobilePhone(mobilePhone));
        [HttpGet("service-request-info")]
        public async Task<ActionResult<ServiceRequestInfoDto>> GetServiceRequestInfo([FromQuery]int id)
            => Ok(await _homeService.GetServiceRequestInfoById(id));

    }
}
