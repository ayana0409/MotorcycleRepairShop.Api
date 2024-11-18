﻿using AutoMapper;
using MotorcycleRepairShop.Application.Interfaces;
using MotorcycleRepairShop.Application.Interfaces.Services;
using MotorcycleRepairShop.Application.Model;
using Serilog;

namespace MotorcycleRepairShop.Infrastructure.Services
{
    public class StatisticService : BaseService, IStatisticService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public StatisticService(ILogger logger, IUnitOfWork unitOfWork, IMapper mapper) : base(logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ServiceRequestStatisticsDto>> GetServiceRequestStatisticsAsync(DateTime startDate, DateTime? endDate)
        {
            var statistics = await _unitOfWork.StatisticRepository
                .GetServiceRequestStatisticByDay(startDate, endDate ?? DateTime.UtcNow);

            _logger.Information($"GetServiceRequestStatistics from {startDate:dd/MM/yyyy} to {endDate:dd/MM/yyyy}");
            return statistics;
        }

        public async Task<decimal> GetRevenueStatisticsAsync(DateTime startDate, DateTime? endDate)
        {
            var result = await _unitOfWork.StatisticRepository
                .GetRevenueStatisticByDay(startDate, endDate ?? DateTime.UtcNow);

            _logger.Information($"GetRevenueStatistics from {startDate:dd/MM/yyyy} to {endDate:dd/MM/yyyy}");
            return result;
        }
    }
}