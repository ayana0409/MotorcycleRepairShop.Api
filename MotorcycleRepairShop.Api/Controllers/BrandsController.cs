﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MotorcycleRepairShop.Application.Interfaces.Services;
using MotorcycleRepairShop.Application.Model;
using MotorcycleRepairShop.Application.Model.Service;
using MotorcycleRepairShop.Domain.Entities;

namespace MotorcycleRepairShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandsController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet("pagination")]
        public async Task<ActionResult<TableResponse<BrandTableDto>>> GetPagination(
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 5,
            [FromQuery] string keyword = "")
            => Ok(await _brandService.GetBrandPagination(new TableRequest
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Keyword = keyword
            }));

        [HttpGet("{id}")]
        public async Task<ActionResult<BrandDto>> GetBrandById(int id)
            => Ok(await _brandService.GetById(id));

        [HttpPost]
        public async Task<ActionResult<int>> CreateBrand([FromBody] BrandDto brandDto)
            => Ok(await _brandService.CreateBrand(brandDto));

        [HttpPut("{id}")]
        public async Task<ActionResult<BrandDto>> UpdateBrand(int id, [FromBody] BrandDto brandDto)
            => Ok(await _brandService.UpdateBrand(id, brandDto));

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBrand(int id)
        {
            await _brandService.DeleteBrand(id);
            return NoContent();
        }
    }
}
