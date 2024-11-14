using MotorcycleRepairShop.Application.Interfaces.Repositories;
using MotorcycleRepairShop.Domain.Entities;
using MotorcycleRepairShop.Infrastructure.Persistence;

namespace MotorcycleRepairShop.Infrastructure.Repositories
{
    public class BrandRepository : BaseRepository<Brand>, IBrandRepository
    {
        public BrandRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {}

        public async Task<(IEnumerable<Brand>, int)> GetPanigationAsync(int pageIndex, int pageSize, string keyword) 
        {
            var query = await base.GetAllAsync();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(c => c.IsActive == true &&
                                        c.Name != null &&
                                        (c.Name.Contains(keyword, StringComparison.CurrentCultureIgnoreCase)
                                        || c.Id.ToString().Contains(keyword, StringComparison.CurrentCultureIgnoreCase)
                                        || c.Country.Contains(keyword, StringComparison.CurrentCultureIgnoreCase)));
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
        public async Task<bool> AnyAsync(int partId)
            => await base.GetSigleAsync(r => r.Id.Equals(partId)).ConfigureAwait(false) != null;
    }
}
