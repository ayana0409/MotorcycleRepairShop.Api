using Microsoft.AspNetCore.Identity;
using MotorcycleRepairShop.Domain.Entities;

namespace MotorcycleRepairShop.Application.Interfaces.Repositories
{
    public interface IAccountRepository : IBaseRepository<ApplicationUser>
    {
        Task<(IEnumerable<ApplicationUser>, int)> GetAdminAccountPanigationAsync(int pageIndex, int pageSize, string keyword);
        Task<(IEnumerable<ApplicationUser>, int)> GetCustomerAccountPanigationAsync(int pageIndex, int pageSize, string keyword);
        Task<IEnumerable<IdentityUserRole<string>>> GetRoles();
    }
}
