using MotorcycleRepairShop.Application.Interfaces.Repositories;
using MotorcycleRepairShop.Domain.Entities;
using MotorcycleRepairShop.Infrastructure.Persistence;

namespace MotorcycleRepairShop.Infrastructure.Repositories
{
    public class ServiceRequestItemRepository : BaseRepository<ServiceRequestItem>, IServiceRequestItemRepository
    {
        public ServiceRequestItemRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }

        public async Task<ServiceRequestItem?> GetByServiceRequestIdAndServiceId(int serviceRequestId, int serviceId)
            => await base.GetSigleAsync(ri => ri.ServiceId.Equals(serviceId) && ri.ServiceRequestId.Equals(serviceRequestId));
    }
}
