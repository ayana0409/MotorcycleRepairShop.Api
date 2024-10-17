using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MotorcycleRepairShop.Application.Interfaces.Services;
using MotorcycleRepairShop.Application.Model.Account;
using MotorcycleRepairShop.Domain.Entities;

namespace MotorcycleRepairShop.Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public AccountService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<CreateAccountDto> CreateAccount(CreateAccountDto account)
        {

            var user = _mapper.Map<ApplicationUser>(account);

            user.UserName = user.Email;
            var addedAccount = await _userManager.CreateAsync(user, account.Password);

            if (addedAccount.Succeeded)
            {
                await _userManager.AddToRolesAsync(user, account.UserRoles);
                var result = _mapper.Map<CreateAccountDto>(account);
                return result;
            }
            else
            {
                var errorMessage = string.Join(", ", addedAccount.Errors.Select(e => e.Description));
                throw new ApplicationException($"Error while creating account: {errorMessage}");
            }
            
        }
    }
}
