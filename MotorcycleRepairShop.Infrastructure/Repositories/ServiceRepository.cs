﻿using Microsoft.EntityFrameworkCore;
using MotorcycleRepairShop.Application.Interfaces.Repositories;
using MotorcycleRepairShop.Domain.Entities;
using MotorcycleRepairShop.Infrastructure.Persistence;

namespace MotorcycleRepairShop.Infrastructure.Repositories
{
    public class ServiceRepository : BaseRepository<Service>,IServiceRepository
    {
        private readonly ApplicationDbContext _context;
        public ServiceRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task<(IEnumerable<Service>, int)> GetPanigationAsync(int pageIndex, int pageSize, string keyword)
        {
            var query = await base.GetAllAsync();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(c => c.IsActive == true &&
                                        c.Name != null &&
                                        (c.Name.Contains(keyword, StringComparison.CurrentCultureIgnoreCase)
                                        || c.Id.ToString().Contains(keyword, StringComparison.CurrentCultureIgnoreCase)));
            }
            else
            {
                query = query.Where(p => p.IsActive == true);
            }

            var data = query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToList();

            int total = query.Count();

            return (data, total);
        }

        public async Task<IEnumerable<Service>> GetByServiceRequestId(int serviceRequestId)
            => await _context.Services
                .Include(x => x.Requests)
                .Where(s => s.Requests.Any(sp => sp.ServiceRequestId.Equals(serviceRequestId)))
                .ToListAsync();
    }
}
