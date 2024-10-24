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
            => Ok(await _serviceRequestService.CreateRescueRescueRequest(serviceRequestDto));

        [HttpPut("service/{id}")]
        public async Task<ActionResult<ServiceRequestItemDto>> UpSertServiceRequestItem(int id, UpsSertServiceRequestItemDto serviceRequestItemDto)
            => Ok(await _serviceRequestService.UpSertServiceToServiceRequest(id, serviceRequestItemDto));
    }
}
