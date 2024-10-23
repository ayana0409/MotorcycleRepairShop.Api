using Microsoft.EntityFrameworkCore;
using MotorcycleRepairShop.Application.Interfaces.Repositories;
using MotorcycleRepairShop.Domain.Entities;
using MotorcycleRepairShop.Infrastructure.Persistence;

namespace MotorcycleRepairShop.Infrastructure.Repositories
{
    public class ServiceRequestRepository : BaseRepository<ServiceRequest>, IServiceRequestRepository
    {
       // private readonly ApplicationDbContext _context;
        public ServiceRequestRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            //_context = applicationDbContext;
        }

        //public async Task<ServiceRequest?> GetById(int id)
        //    => await _context.ServiceRequests
        //        .Include(r => r.Images)
        //        .Include(r => r.Videos)
        //        .Include(r => r.Problems)
        //        .FirstOrDefaultAsync(r => r.Id.Equals(id));
    }
}
