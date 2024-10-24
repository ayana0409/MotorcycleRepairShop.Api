using MotorcycleRepairShop.Domain.Entities;

namespace MotorcycleRepairShop.Application.Interfaces.Repositories
{
    public interface IServiceRequestItemRepository : IBaseRepository<ServiceRequestItem>
    {
        Task<ServiceRequestItem?> GetByServiceRequestIdAndServiceId(int serviceRequestId, int serviceId);
    }
}
