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
    }
}
