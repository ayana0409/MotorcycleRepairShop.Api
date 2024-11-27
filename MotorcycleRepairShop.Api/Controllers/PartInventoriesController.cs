using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotorcycleRepairShop.Application.Interfaces.Services;
using MotorcycleRepairShop.Application.Model;

namespace MotorcycleRepairShop.Api.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
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
        public async Task<ActionResult<List<CreatePartInventoryDto>>> GetAvailableInventories(int partId)
            => Ok(await _partInventoryService.GetAvailableInventoriesByPartId(partId));

        /// <summary>
        /// Nhập hàng cho 1 linh kiện
        /// </summary>
        /// <param name="partInventoryDto">Thông tin nhập hàng</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> CreatePartInventory(CreatePartInventoryDto partInventoryDto)
            => Ok(await _partInventoryService.CreatePartInventory(partInventoryDto));

        /// <summary>
        /// Nhập hàng cho nhiều linh kiện cùng lúc
        /// </summary>
        /// <param name="partInventoryDtos">Danh sách các thông tin nhập hàng</param>
        /// <returns></returns>
        [HttpPost("multiple")]
        public async Task<ActionResult<List<int>>> CreateMultipleInventories(List<CreatePartInventoryDto> partInventoryDtos)
            => Ok(await _partInventoryService.CreatePartInventories(partInventoryDtos));
    }
}
