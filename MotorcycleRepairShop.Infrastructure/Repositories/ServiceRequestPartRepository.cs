using Microsoft.EntityFrameworkCore;
using MotorcycleRepairShop.Application.Interfaces.Repositories;
using MotorcycleRepairShop.Domain.Entities;
using MotorcycleRepairShop.Infrastructure.Persistence;

namespace MotorcycleRepairShop.Infrastructure.Repositories
{
    public class ServiceRequestPartRepository : BaseRepository<ServiceRequestPart>, IServiceRequestPartRepository
    {
        private readonly ApplicationDbContext _context;
        public ServiceRequestPartRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task<ServiceRequestPart?> GetByServiceRequestIdAndPartId(int serviceRequestId, int partId)
            => await GetSigleAsync(ri => ri.PartId.Equals(partId) && ri.ServiceRequestId.Equals(serviceRequestId));
        public async Task<IEnumerable<ServiceRequestPart>> GetByServiceRequestId(int serviceRequestId)
            => await _context.Set<ServiceRequestPart>()
            .Include(x => x.Part)
            .Where(ri => ri.ServiceRequestId.Equals(serviceRequestId))
            .ToListAsync();
    }
}
