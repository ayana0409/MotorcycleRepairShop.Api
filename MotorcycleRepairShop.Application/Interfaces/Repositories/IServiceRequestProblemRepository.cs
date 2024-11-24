using MotorcycleRepairShop.Domain.Entities;

namespace MotorcycleRepairShop.Application.Interfaces.Repositories
{
    public interface IServiceRequestProblemRepository : IBaseRepository<ServiceRequestProblem>
    {
        Task<ServiceRequestProblem?> GetByServiceRequestIdAndProblemId(int serviceRequestId, int problemId);
    }
}
