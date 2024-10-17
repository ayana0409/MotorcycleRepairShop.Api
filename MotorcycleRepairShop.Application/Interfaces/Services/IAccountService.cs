using MotorcycleRepairShop.Application.Model.Account;

namespace MotorcycleRepairShop.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<CreateAccountDto> CreateAccount(CreateAccountDto account);
    }
}