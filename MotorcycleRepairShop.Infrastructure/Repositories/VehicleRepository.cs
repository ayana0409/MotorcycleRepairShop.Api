using Microsoft.EntityFrameworkCore;
using MotorcycleRepairShop.Application.Interfaces.Repositories;
using MotorcycleRepairShop.Domain.Entities;
using MotorcycleRepairShop.Infrastructure.Persistence;

namespace MotorcycleRepairShop.Infrastructure.Repositories
{
    public class VehicleRepository : BaseRepository<Vehicle>, IVehicleRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public VehicleRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<(IEnumerable<Vehicle>, int)> GetPanigationAsync(int pageIndex, int pageSize, string keyword)
        {
            var query = await _applicationDbContext.Set<Vehicle>()
                .Include(v => v.Images)
                .Include(v => v.Brand)
                .ToListAsync();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(c => c.IsActive == true &&
                                        c.Name != null &&
                                        (c.Name.Contains(keyword, StringComparison.CurrentCultureIgnoreCase)
                                        || c.Id.ToString().Contains(keyword, StringComparison.CurrentCultureIgnoreCase)))
                                        .ToList();
            }
            else
            {
                query = query.Where(p => p.IsActive == true).ToList();
            }

            var data = query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToList();

            int total = query.Count;

            return (data, total);
        }

        public async Task<IEnumerable<Vehicle>> GetVehiclesByBrandId(int brandId)
        {
            var query = await _applicationDbContext.Set<Vehicle>()
                .Include(v => v.Images)
                .Include(v => v.Brand)
                .AsSplitQuery()
                .ToListAsync();
            var result = query
                .Where(v => v.IsActive == true && v.BrandId.Equals(brandId))
                .ToList();
            return result;
        }

        public async Task<Vehicle?> GetById(int id)
        {
             var query = await _applicationDbContext.Set<Vehicle>()
                .Include(v => v.Images)
                .Include(v => v.Brand)
                .AsSplitQuery()
                .ToListAsync();
            var result = query
                .FirstOrDefault(v => v.IsActive == true && v.Id.Equals(id));
            return result;
        }
    }
}
