using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MotorcycleRepairShop.Application.Interfaces;
using MotorcycleRepairShop.Application.Interfaces.Services;
using MotorcycleRepairShop.Application.Model;
using MotorcycleRepairShop.Application.Model.Account;
using MotorcycleRepairShop.Domain.Entities;
using MotorcycleRepairShop.Share.Exceptions;
using PayPal.Api;
using Serilog;

namespace MotorcycleRepairShop.Infrastructure.Services
{
    public class AccountService : BaseService, IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AccountService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, ILogger logger, IUnitOfWork unitOfWork) : base(logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<TableResponse<AccountTableDto>> GetCustomerAccountPagination(TableRequest request)
        {
            var (result, total) = await _unitOfWork.AccountRepository
                .GetCustomerAccountPanigationAsync(request.PageIndex, request.PageSize, request.Keyword ?? "");

            var datas = _mapper.Map<IEnumerable<AccountTableDto>>(result);
            return new TableResponse<AccountTableDto>
            {
                PageSize = request.PageSize,
                Datas = datas,
                Total = total
            };
        }

        public async Task<TableResponse<AccountTableDto>> GetAdminAccountPagination(TableRequest request)
        {
            var (result, total) = await _unitOfWork.AccountRepository
                .GetAdminAccountPanigationAsync(request.PageIndex, request.PageSize, request.Keyword ?? "");

            List<AccountTableDto> datas = [];

            foreach (var item in result)
            {
                var account = _mapper.Map<AccountTableDto>(item);
                account.UserRoles = (await _userManager.GetRolesAsync(item)).ToList();
                datas.Add(account);
            }

            return new TableResponse<AccountTableDto>
            {
                PageSize = request.PageSize,
                Datas = datas,
                Total = total
            };
        }

        public async Task<AccountInfoDto> GetAccountByUsername(string username)
        {
            var user = await FindAccountByUsername(username);
            var result = _mapper.Map<AccountInfoDto>(user);
            return result;
        }

        public async Task<CreateAccountDto> CreateAccount(CreateAccountDto account) 
        {
            var user = _mapper.Map<ApplicationUser>(account);
            user.UserName = user.MobilePhone;

            var addedAccount = await _userManager.CreateAsync(user, account.Password);

            if (addedAccount.Succeeded)
            {
                var serviceRequests = await _unitOfWork.ServiceRequestRepository
                .GetAllAsync(s => s.MobilePhone.Equals(user.MobilePhone));
                if (serviceRequests.Any())
                {
                    foreach (var serviceRequest in serviceRequests)
                        serviceRequest.CustomerId = user.Id;
                    await _unitOfWork.SaveChangeAsync();
                }

                await _userManager.AddToRolesAsync(user, account.UserRoles);
                var result = _mapper.Map<CreateAccountDto>(account);
                _logger.Information($"CreateAccount successfuly: {user.UserName}");
                return result;
            }
            else
            {
                var errorMessage = string.Join(", ", addedAccount.Errors.Select(e => e.Description));
                _logger.Information($"CreateAccount failed: {user.UserName}");
                throw new ApplicationException($"Error while creating account: {errorMessage}");
            }
        }

        public async Task<AccountInfoDto> UpdateAccountInfo(string username, AccountInfoDto accountInfo)
        {
            var account = await FindAccountByUsername(username);
            account = _mapper.Map(accountInfo, account);
            await _userManager.UpdateAsync(account);

            LogSuccess(username);
            var result = _mapper.Map<AccountInfoDto>(account);
            return result;
        }

        public async Task<bool> UpdateAccountRole(string username, IEnumerable<string> roles)
        {
            var account = await FindAccountByUsername(username);


            var allRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            var invalidRoles = roles.Where(role => !allRoles.Contains(role)).ToList();
            if (invalidRoles.Count != 0)
            {
                throw new ArgumentException($"The following roles are invalid: {string.Join(", ", invalidRoles)}");
            }

            var currentRoles = await _userManager.GetRolesAsync(account);
            await _userManager.RemoveFromRolesAsync(account, currentRoles);

            var addRolesResult = await _userManager.AddToRolesAsync(account, roles);
            if (!addRolesResult.Succeeded)
            {
                await _userManager.AddToRolesAsync(account, currentRoles);
                _logger.Information($"Failed to add new roles. - {username}");
                return false;
            }
            LogSuccess(username);
            return true;
        }

        public async Task UpdateAccountPassword(string username, string password)
        {
            var account = await FindAccountByUsername(username);
            var passwordValidator = await _userManager.PasswordValidators.First().ValidateAsync(_userManager, account, password);
            if (passwordValidator != null && !passwordValidator.Succeeded)
            {
                var errors = string.Join(", ", passwordValidator.Errors.Select(e => e.Description));
                throw new ArgumentException($"Invalid password: {errors}");
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(account);
            await _userManager.ResetPasswordAsync(account, resetToken, password);
            LogSuccess(username);
        }

        public async Task DeleteAccount(string username)
        {
            var account = await FindAccountByUsername(username);
            account.IsActive = false;
            await _userManager.UpdateAsync(account);
        }

        public async Task<IEnumerable<string?>> GetUserRoles()
            => await _roleManager.Roles.Select(r => r.Name).ToListAsync();

        private async Task<ApplicationUser> FindAccountByUsername(string username)
            => await _userManager.FindByNameAsync(username)
                ?? throw new NotFoundException(nameof(ApplicationUser), username);
    }
}
