﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotorcycleRepairShop.Application.Interfaces.Services;
using MotorcycleRepairShop.Application.Model;
using System.ComponentModel.DataAnnotations;

namespace MotorcycleRepairShop.Api.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticController : ControllerBase
    {
        private readonly IStatisticService _statisticService;

        public StatisticController(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }

        /// <summary>
        /// Thống kê số lượng từng loại Service Request theo khoảng ngày
        /// </summary>
        /// <param name="startDate">Ngày bắt đầu (yyyy-MM-dd)</param>
        /// <param name="endDate">Ngày kết thúc (yyyy-MM-dd) - mặc định hôm nay</param>
        /// <returns></returns>
        [HttpGet("service-request")]
        public async Task<ActionResult<IEnumerable<ServiceRequestStatisticsDto>>> 
            GetServiceRequestStatisticsByDate([Required] DateTime startDate, DateTime? endDate)
            => Ok(await _statisticService.GetServiceRequestStatisticsAsync(startDate, endDate));

        /// <summary>
        /// Thống kê doanh thu, chi phí nhập vào, tiền thuế phải chịu khi nhập hàng theo khoảng ngày
        /// </summary>
        /// <param name="startDate">Ngày bắt đầu (yyyy-MM-dd)</param>
        /// <param name="endDate">Ngày kết thúc (yyyy-MM-dd) - mặc định hôm nay</param>
        /// <returns></returns>
        [HttpGet("revenue")]
        public async Task<ActionResult<StatisticDto>>
            GetRevenueStatisticsByDate([Required] DateTime startDate, DateTime? endDate)
            => Ok(await _statisticService.GetStatisticsAsync(startDate, endDate));
    }
}
