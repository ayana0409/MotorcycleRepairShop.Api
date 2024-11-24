using Microsoft.AspNetCore.Mvc;
using MotorcycleRepairShop.Application.Interfaces.Services;
using MotorcycleRepairShop.Application.Model;
using MotorcycleRepairShop.Application.Model.Account;

namespace MotorcycleRepairShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// Lấy danh sách tài khoản của khách hàng
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet("customer/pagination")]
        public async Task<ActionResult<TableResponse<BrandTableDto>>> GetCustomerAccountPagination(
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 5,
            [FromQuery] string keyword = "")
            => Ok(await _accountService.GetCustomerAccountPagination(new TableRequest
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Keyword = keyword
            }));

        /// <summary>
        /// Lấy danh sách tài khoản của nhân viên cửa hàng
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet("admin/pagination")]
        public async Task<ActionResult<TableResponse<BrandTableDto>>> GetAdminAccountPagination(
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 5,
            [FromQuery] string keyword = "")
            => Ok(await _accountService.GetAdminAccountPagination(new TableRequest
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Keyword = keyword
            }));

        /// <summary>
        /// Lấy thông tin tài khoản bằng username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet("{username}")]
        public async Task<ActionResult<AccountInfoDto>> GetAccountInfoByUsername(string username)
            => Ok(await _accountService.GetAccountByUsername(username));

        /// <summary>
        /// Lấy danh sách các quyền
        /// </summary>
        /// <returns></returns>
        [HttpGet("roles")]
        public async Task<ActionResult<IEnumerable<string>>> GetRoles()
            => Ok(await _accountService.GetUserRoles());

        /// <summary>
        /// Tạo tài khoản
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<CreateAccountDto>> CreateAccount(CreateAccountDto account)
            => Ok(await _accountService.CreateAccount(account));

        /// <summary>
        /// Cập nhật thông tin cá nhân cho tài khoản
        /// </summary>
        /// <param name="username">Username cần cập nhật</param>
        /// <param name="accountInfo">Các thông tin cần cập nhật</param>
        /// <returns></returns>
        [HttpPut("infos/{username}")]
        public async Task<ActionResult<AccountInfoDto>> UpdateAccountInfo(string username, AccountInfoDto accountInfo)
            => Ok(await _accountService.UpdateAccountInfo(username, accountInfo));

        /// <summary>
        /// Cập nhật quyền cho tài khoản
        /// </summary>
        /// <param name="username">Username cần cập nhật</param>
        /// <param name="roles">Danh sách các quyền của tài khoản</param>
        /// <returns></returns>
        [HttpPatch("roles/{username}")]
        public async Task<ActionResult> UpdateAccountRole(string username, IEnumerable<string> roles)
            => await _accountService.UpdateAccountRole(username, roles) 
            ? Ok($"Update {username} roles successfuly") 
            : BadRequest($"Update {username} roles failed");

        /// <summary>
        /// Cập nhật lại mật khẩu cho tài khoản
        /// </summary>
        /// <param name="username">Username cần cập nhật</param>
        /// <param name="password">Mật khẩu mới</param>
        /// <returns></returns>
        [HttpPatch("password/{username}")]
        public async Task<ActionResult> UpdateAccountPassword(string username, string password)
        {
            await _accountService.UpdateAccountPassword(username, password);
            return Ok();
        }
    }
}
