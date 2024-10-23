using Microsoft.EntityFrameworkCore;
using MotorcycleRepairShop.Application.Interfaces.Repositories;
using MotorcycleRepairShop.Domain.Entities;
using MotorcycleRepairShop.Infrastructure.Persistence;

namespace MotorcycleRepairShop.Infrastructure.Repositories
{
    public class ProblemRepository : BaseRepository<Problem>, IProblemRepository
    {
        private readonly ApplicationDbContext _context;
        public ProblemRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _context = applicationDbContext;
        }
        public async Task<(IEnumerable<Problem>, int)> GetPanigationAsync(int pageIndex, int pageSize, string keyword)
        {
            var query = await base.GetAllAsync();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(c => c.IsActive == true &&
                                        c.Name != null &&
                                        (c.Name.Contains(keyword, StringComparison.CurrentCultureIgnoreCase)
                                        || c.Id.ToString().Contains(keyword, StringComparison.CurrentCultureIgnoreCase)
                                        || c.Description.Contains(keyword, StringComparison.CurrentCultureIgnoreCase)));
            }
            else
            {
                query = query.Where(p => p.IsActive == true);
            }

            var data = query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToList();

            int total = query.Count();

            return (data, total);
        }

        public async Task<IEnumerable<Problem>> GetByServideRequestId(int id)
            => await _context.Problems.Include(p => p.ServiceRequestProblems)
            .Where(p => p.ServiceRequestProblems.Any(sp => sp.ServiceRequestId.Equals(id)))
            .ToListAsync();
            
    }
}
