using Microsoft.EntityFrameworkCore;
using MotorcycleRepairShop.Application.Interfaces.Repositories;
using MotorcycleRepairShop.Application.Model;
using MotorcycleRepairShop.Domain.Entities;
using MotorcycleRepairShop.Infrastructure.Persistence;

namespace MotorcycleRepairShop.Infrastructure.Repositories
{
    public class StatisticRepository : IStatisticRepository
    {
        private readonly ApplicationDbContext _context;

        public StatisticRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ServiceRequestStatisticsDto>> GetServiceRequestStatisticByDayAsync(DateTime startDay, DateTime endDate)
        {
            var result = await _context.Set<ServiceRequest>()
                .Where(sr => sr.DateSubmitted >= startDay && sr.DateSubmitted <= endDate)
                .GroupBy(sr => new { sr.DateSubmitted.Date, sr.ServiceType })
                .Select(g => new
                {
                    Date = g.Key.Date.ToString("yyyy-MM-dd"),
                    ServiceType = g.Key.ServiceType.ToString(),
                    Count = g.Count()
                })
                .ToListAsync();

            var groupedByDate = result
                .GroupBy(r => r.Date)
                .Select(g => new ServiceRequestStatisticsDto
                {
                    Date = g.Key,
                    ServiceTypeCounts = g.ToDictionary(r => r.ServiceType, r => r.Count)
                })
                .ToList();

            return groupedByDate;
        }
    }
}
