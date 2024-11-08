using MotorcycleRepairShop.Application.Model.Account;

namespace MotorcycleRepairShop.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<CreateAccountDto> CreateAccount(CreateAccountDto account);
        Task<AccountInfoDto> GetAccountByUsername(string username);
        Task<AccountInfoDto> UpdateAccountInfo(string username, AccountInfoDto accountInfo);
        Task UpdateAccountPassword(string username, string password);
        Task<bool> UpdateAccountRole(string username, IEnumerable<string> roles);
    }
}