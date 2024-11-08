using Microsoft.EntityFrameworkCore;
using MotorcycleRepairShop.Application.Interfaces.Repositories;
using MotorcycleRepairShop.Domain.Entities;
using MotorcycleRepairShop.Infrastructure.Persistence;

namespace MotorcycleRepairShop.Infrastructure.Repositories
{
    public class PartRepository : BaseRepository<Part>, IPartRepository
    {
        private readonly ApplicationDbContext _context;
        public PartRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task<(IEnumerable<Part>, int)> GetPanigationAsync(int pageIndex, int pageSize, string keyword)
        {
            var query = _context
                .Set<Part>()
                .Include(p => p.Brand)
                .Where(p => p.IsActive == true);

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

        public async Task<IEnumerable<Part>> GetByServiceRequestId(int serviceRequestId)
            => await _context.Parts
                .Include(x => x.ServiceRequests)
                .Where(s => s.ServiceRequests.Any(sp => sp.ServiceRequestId.Equals(serviceRequestId)))
                .ToListAsync();
    }
}
