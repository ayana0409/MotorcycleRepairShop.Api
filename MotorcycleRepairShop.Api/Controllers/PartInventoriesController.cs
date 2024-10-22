using Microsoft.AspNetCore.Mvc;
using MotorcycleRepairShop.Application.Interfaces.Services;
using MotorcycleRepairShop.Application.Model;

namespace MotorcycleRepairShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartInventoriesController : ControllerBase
    {
        private readonly IPartInventoryService _partInventoryService;

        public PartInventoriesController(IPartInventoryService partInventoryService)
        {
            _partInventoryService = partInventoryService;
        }

        [HttpGet("available/{partId}")]
        public async Task<ActionResult<List<PartInventoryDto>>> GetAvailableInventories(int partId)
            => Ok(await _partInventoryService.GetAvailableInventoriesByPartId(partId));

        [HttpPost]
        public async Task<ActionResult<int>> CreatePartInventory(PartInventoryDto partInventoryDto)
            => Ok(await _partInventoryService.CreatePartInventory(partInventoryDto));

        [HttpPost("multiple")]
        public async Task<ActionResult<List<int>>> CreateMultipleInventories(List<PartInventoryDto> partInventoryDtos)
            => Ok(await _partInventoryService.CreatePartInventories(partInventoryDtos));
    }
}
