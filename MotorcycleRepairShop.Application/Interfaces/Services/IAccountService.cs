using MotorcycleRepairShop.Application.Model;
using MotorcycleRepairShop.Application.Model.Account;

namespace MotorcycleRepairShop.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<CreateAccountDto> CreateAccount(CreateAccountDto account);
        Task DeleteAccount(string username);
        Task<AccountInfoDto> GetAccountByUsername(string username);
        Task<TableResponse<AccountTableDto>> GetAdminAccountPagination(TableRequest request);
        Task<TableResponse<AccountTableDto>> GetCustomerAccountPagination(TableRequest request);
        Task<IEnumerable<string?>> GetUserRoles();
        Task<AccountInfoDto> UpdateAccountInfo(string username, AccountInfoDto accountInfo);
        Task UpdateAccountPassword(string username, string password);
        Task<bool> UpdateAccountRole(string username, IEnumerable<string> roles);
    }
}