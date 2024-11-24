using AutoMapper;
using MotorcycleRepairShop.Application.Interfaces;
using MotorcycleRepairShop.Application.Interfaces.Services;
using MotorcycleRepairShop.Application.Model;
using Serilog;

namespace MotorcycleRepairShop.Infrastructure.Services
{
    public class StatisticService : BaseService, IStatisticService
    {
        private readonly IUnitOfWork _unitOfWork;
        public StatisticService(ILogger logger, IUnitOfWork unitOfWork) : base(logger)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ServiceRequestStatisticsDto>> GetServiceRequestStatisticsAsync(DateTime startDate, DateTime? endDate)
        {
            var statistics = await _unitOfWork.StatisticRepository
                .GetServiceRequestStatisticByDay(startDate, endDate ?? DateTime.UtcNow);

            _logger.Information($"GetServiceRequestStatistics from {startDate:dd/MM/yyyy} to {endDate:dd/MM/yyyy}");
            return statistics;
        }

        public async Task<StatisticDto> GetStatisticsAsync(DateTime startDate, DateTime? endDate)
        {
            var revenue = await _unitOfWork.StatisticRepository
                .GetRevenueStatisticByDay(startDate, endDate ?? DateTime.UtcNow);

            var cost = await _unitOfWork.StatisticRepository
                .GetCostStatisticByDay(startDate, endDate ?? DateTime.UtcNow);

            var taxCost = await _unitOfWork.StatisticRepository
                .GetTaxCostStatisticByDay(startDate, endDate ?? DateTime.UtcNow);

            _logger.Information($"GetStatistics from {startDate:dd/MM/yyyy} to {endDate:dd/MM/yyyy}");
            return new()
            {
                Revenue = revenue,
                Cost = cost,
                TaxCost = taxCost
            };
        }
    }
}
