using Microsoft.EntityFrameworkCore;
using MotorcycleRepairShop.Application.Interfaces.Repositories;
using MotorcycleRepairShop.Domain.Entities;
using MotorcycleRepairShop.Infrastructure.Persistence;

namespace MotorcycleRepairShop.Infrastructure.Repositories
{
    public class ServiceRequestRepository : BaseRepository<ServiceRequest>, IServiceRequestRepository
    {
        private readonly ApplicationDbContext _context;
        public ServiceRequestRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _context = applicationDbContext;
        }
        public IQueryable<ServiceRequest> GetQueryable() 
            => _context.Set<ServiceRequest>().AsQueryable();
        public async Task<(IEnumerable<ServiceRequest>, int)> GetPanigationAsync(int pageIndex, int pageSize, string keyword)
        {
            var query = _context.Set<ServiceRequest>()
                .Include(sr => sr.Services)
                .Include(sr => sr.Parts)
                .AsSplitQuery()
                .AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(c => c.MobilePhone.Contains(keyword, StringComparison.CurrentCultureIgnoreCase)
                                         || c.FullName.Contains(keyword, StringComparison.CurrentCultureIgnoreCase)
                                         || c.Address.Contains(keyword, StringComparison.CurrentCultureIgnoreCase)
                                         || c.Id.ToString().Contains(keyword));
            }

            int total = await query.CountAsync();

            var data = await query
                .OrderBy(c => c.Id)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (data, total);
        }

        public async Task<ServiceRequest?> GetById(int id)
            => await _context.Set<ServiceRequest>()
                .Include(sr => sr.Images)
                .Include(sr => sr.Videos)
                .Include(sr => sr.Services)
                .Include(sr => sr.Parts)
                .AsSplitQuery()
                .FirstOrDefaultAsync(sr => sr.Id.Equals(id));

        public async Task<IEnumerable<ServiceRequest>> GetByUsername(string username)
            => await _context.Set<ServiceRequest>()
                .Include(s => s.Parts)
                .Include(s => s.Services)
                .Include(s => s.Customer)
                .AsSplitQuery()
                .Where(s => s.Customer != null
                    && s.Customer.UserName != null
                    && s.Customer.UserName.Equals(username))
                .ToListAsync();

        public async Task<IEnumerable<ServiceRequest>> GetByMobilePhone(string mobilePhone)
            => await _context.Set<ServiceRequest>()
            .Include(s => s.Parts)
            .Include(s => s.Services)
            .AsSplitQuery()
            .Where(s => s.MobilePhone.Equals(mobilePhone))
            .ToListAsync();

        public async Task<bool> AnyAsync(int serviceRequestId)
            => await base.GetSigleAsync(r => r.Id.Equals(serviceRequestId)) != null;
    }
}
