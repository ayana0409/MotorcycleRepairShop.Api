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

        public async Task<IEnumerable<ServiceRequest>> GetByUsername(string username)
            => await _context.Set<ServiceRequest>()
                .Include(s => s.Customer)
                .Where(s => s.Customer != null
                    && s.Customer.UserName != null
                    && s.Customer.UserName.Equals(username))
                .ToListAsync();

        public async Task<bool> AnyAsync(int serviceRequestId)
            => await base.GetSigleAsync(r => r.Id.Equals(serviceRequestId)) != null;
    }
}
