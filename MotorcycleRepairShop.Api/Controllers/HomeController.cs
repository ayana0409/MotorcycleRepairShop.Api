using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotorcycleRepairShop.Application.Interfaces.Services;
using MotorcycleRepairShop.Application.Model;
using MotorcycleRepairShop.Application.Model.Account;
using MotorcycleRepairShop.Application.Model.Problem;
using MotorcycleRepairShop.Domain.Entities;
using MotorcycleRepairShop.Infrastructure.Services;

namespace MotorcycleRepairShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IHomeService _homeService;
        private readonly IProblemService _problemService;
        private readonly IPartService _partService;
        private readonly IServiceService _serviceService;

        public HomeController(IHomeService homeService, IProblemService problemService, IPartService partService, IServiceService serviceService)
        {
            _homeService = homeService;
            _problemService = problemService;
            _partService = partService;
            _serviceService = serviceService;
        }
        [Authorize]
        [HttpGet("user-info")]
        public async Task<ActionResult<AccountInfoDto>> GetUserInfo()
        {
            var username = User.Identity?.Name
                ?? User.Claims.FirstOrDefault()?.Value;
            if (username == null) return Unauthorized();

            return Ok(await _homeService.GetAccountInfoByUsername(username));
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

        [Authorize]
        [HttpGet("service-requests-by-username")]
        public async Task<ActionResult<IEnumerable<ServiceRequestHomeDto>>>
            GetServiceRequestByUsername()
        {
            var username = User.Identity?.Name 
                ?? User.Claims.FirstOrDefault()?.Value;
            if (string.IsNullOrEmpty(username))
                return Unauthorized();
            return Ok(await _homeService.GetServiceRequestsByUsername(username));
        }

        [HttpGet("service-request-info")]
        public async Task<ActionResult<ServiceRequestInfoDto>> GetServiceRequestInfo([FromQuery]int id)
            => Ok(await _homeService.GetServiceRequestInfoById(id));

        [HttpGet("problem-dropdown")]
        public async Task<ActionResult<IEnumerable<ProblemForDropdownDto>>> GetProblemForDropDownList()
            => Ok(await _problemService.GetProblemsForDropDownList());

        [HttpGet("parts-dropdown")]
        public async Task<ActionResult<IEnumerable<PartForDropdownDto>>> GetPartForDropDownList()
            => Ok(await _partService.GetPartsForDropDownList());

        [HttpGet("service-dropdown")]
        public async Task<ActionResult<IEnumerable<ServiceForDropdownDto>>> GetServiceForDropDownList()
            => Ok(await _serviceService.GetServicesForDropDownList());

    }
}
