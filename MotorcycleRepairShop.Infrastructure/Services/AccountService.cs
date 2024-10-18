using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MotorcycleRepairShop.Application.Interfaces.Services;
using MotorcycleRepairShop.Application.Model.Account;
using MotorcycleRepairShop.Domain.Entities;
using Serilog;

namespace MotorcycleRepairShop.Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public AccountService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, ILogger logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CreateAccountDto> CreateAccount(CreateAccountDto account)
        {
            _logger.Information($"START - Create account: {account.Email}");
            var user = _mapper.Map<ApplicationUser>(account);

            user.UserName = user.Email;
            var addedAccount = await _userManager.CreateAsync(user, account.Password);

            if (addedAccount.Succeeded)
            {
                await _userManager.AddToRolesAsync(user, account.UserRoles);
                var result = _mapper.Map<CreateAccountDto>(account);
                _logger.Information($"END - Success - Create account: {user.UserName}");
                return result;
            }
            else
            {
                var errorMessage = string.Join(", ", addedAccount.Errors.Select(e => e.Description));
                _logger.Information($"END - Fail - Create account: {user.UserName}");
                throw new ApplicationException($"Error while creating account: {errorMessage}");
            }
            
        }
    }
}
