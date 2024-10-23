using Microsoft.AspNetCore.Http;
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
        public async Task<ActionResult<ServiceRequestDto>> GetById(int id)
            => Ok(await _serviceRequestService.GetServiceRequestById(id));

        [HttpPost]
        public async Task<ActionResult<int>> CreateRequestService(ServiceRequestDto serviceRequestDto)
            => Ok(await _serviceRequestService.CreateDeirecServiceRequest(serviceRequestDto));
    }
}
