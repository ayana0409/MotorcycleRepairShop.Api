﻿using Microsoft.AspNetCore.Mvc;
using MotorcycleRepairShop.Application.Interfaces.Services;
using MotorcycleRepairShop.Application.Model;
using MotorcycleRepairShop.Application.Model.Service;
using MotorcycleRepairShop.Domain.Entities;

namespace MotorcycleRepairShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartsController : ControllerBase
    {
        private readonly IPartService _partService;

        public PartsController(IPartService partService)
        {
            _partService = partService;
        }

        [HttpGet("pagination")]
        public async Task<ActionResult<TableResponse<ServiceTableDto>>> GetPagination(
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 5,
            [FromQuery] string keyword = "")
            => Ok(await _partService.GetPartPagination(new TableRequest
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Keyword = keyword
            }));

        [HttpGet("{id}")]
        public async Task<ActionResult<PartDto>> GetPartById(int id)
            => Ok(await _partService.GetPartById(id));

        [HttpPost]
        public async Task<ActionResult<int>> CreatePart([FromBody] PartDto partDto)
            => Ok(await _partService.CreatePart(partDto));

        [HttpPut("{id}")]
        public async Task<ActionResult<PartDto>> UpdatePart(int id, PartDto partDto) 
            => Ok(await _partService.UpdatePart(id, partDto));

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePart(int id)
        {
            await _partService.DeletePart(id);
            return NoContent();
        }
    }
}
