using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MotorcycleRepairShop.Application.Interfaces.Repositories;
using MotorcycleRepairShop.Domain.Entities;
using MotorcycleRepairShop.Infrastructure.Persistence;

namespace MotorcycleRepairShop.Infrastructure.Repositories
{
    public class AccountRepository : BaseRepository<ApplicationUser>, IAccountRepository
    {
        private readonly ApplicationDbContext _context;
        public AccountRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task<(IEnumerable<ApplicationUser>, int)> GetCustomerAccountPanigationAsync(int pageIndex, int pageSize, string keyword)
        {
            var userRoles = _context.UserRoles.Select(ur => ur.UserId);
            var query = _context.ApplicationUsers
                .Where(c => c.IsActive == true && !userRoles.Contains(c.Id))
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(c => c.UserName != null &&
                                        (c.UserName.Contains(keyword, StringComparison.CurrentCultureIgnoreCase)
                                        || c.MobilePhone.Contains(keyword, StringComparison.CurrentCultureIgnoreCase)));
            }

            var data = await query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            int total = await query.CountAsync();

            return (data, total);
        }

        public async Task<(IEnumerable<ApplicationUser>, int)> GetAdminAccountPanigationAsync(int pageIndex, int pageSize, string keyword)
        {
            var userRoles = _context.UserRoles.Select(ur => ur.UserId);
            var query = _context.ApplicationUsers
                .AsNoTracking()
                .Where(c => c.IsActive == true && userRoles.Contains(c.Id))
                .AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(c => c.UserName != null &&
                                        (c.UserName.Contains(keyword, StringComparison.CurrentCultureIgnoreCase)
                                        || c.MobilePhone.Contains(keyword, StringComparison.CurrentCultureIgnoreCase)));
            }

            var data = await query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            int total = await query.CountAsync();

            return (data, total);
        }

        public async Task<IEnumerable<IdentityUserRole<string>>> GetRoles()
            => await _context.UserRoles.ToListAsync();
    }
}
