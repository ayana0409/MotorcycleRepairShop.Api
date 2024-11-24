using MotorcycleRepairShop.Application.Interfaces.Repositories;
using MotorcycleRepairShop.Domain.Entities;
using MotorcycleRepairShop.Infrastructure.Persistence;

namespace MotorcycleRepairShop.Infrastructure.Repositories
{
    public class ServiceRequestProblemRepository : BaseRepository<ServiceRequestProblem>, IServiceRequestProblemRepository
    {
        public ServiceRequestProblemRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }

        public async Task<ServiceRequestProblem?> GetByServiceRequestIdAndProblemId(int serviceRequestId, int problemId)
            => await base.GetSigleAsync(sp => sp.ServiceRequestId.Equals(serviceRequestId)
            && sp.ProblemId.Equals(problemId));
    }
}
