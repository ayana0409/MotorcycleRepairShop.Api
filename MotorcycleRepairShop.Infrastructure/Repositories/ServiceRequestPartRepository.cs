using MotorcycleRepairShop.Application.Interfaces.Repositories;
using MotorcycleRepairShop.Domain.Entities;
using MotorcycleRepairShop.Infrastructure.Persistence;

namespace MotorcycleRepairShop.Infrastructure.Repositories
{
    public class ServiceRequestPartRepository : BaseRepository<ServiceRequestPart>, IServiceRequestPartRepository
    {
        public ServiceRequestPartRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }

        public async Task<ServiceRequestPart?> GetByServiceRequestIdAndPartId(int serviceRequestId, int partId)
            => await GetSigleAsync(ri => ri.PartId.Equals(partId) && ri.ServiceRequestId.Equals(serviceRequestId));
    }
}
