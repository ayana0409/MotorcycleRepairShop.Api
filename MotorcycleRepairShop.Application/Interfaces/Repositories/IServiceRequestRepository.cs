using MotorcycleRepairShop.Domain.Entities;

namespace MotorcycleRepairShop.Application.Interfaces.Repositories
{
    public interface IServiceRequestRepository : IBaseRepository<ServiceRequest>
    {
        Task<bool> AnyAsync(int serviceRequestId);
        Task<ServiceRequest?> GetById(int id);
        Task<IEnumerable<ServiceRequest>> GetByMobilePhone(string mobilePhone);
        Task<IEnumerable<ServiceRequest>> GetByUsername(string username);
        Task<(IEnumerable<ServiceRequest>, int)> GetPanigationAsync(int pageIndex, int pageSize, string keyword);
        IQueryable<ServiceRequest> GetQueryable();
    }
}
