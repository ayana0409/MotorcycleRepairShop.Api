using MotorcycleRepairShop.Domain.Entities;

namespace MotorcycleRepairShop.Application.Interfaces.Repositories
{
    public interface IServiceRequestPartRepository : IBaseRepository<ServiceRequestPart>
    {
        Task<IEnumerable<ServiceRequestPart>> GetByServiceRequestId(int serviceRequestId);
        Task<ServiceRequestPart?> GetByServiceRequestIdAndPartId(int serviceRequestId, int partId);
    }
}
