using Microsoft.AspNetCore.Mvc;
using MotorcycleRepairShop.Application.Interfaces.Services;
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

        [HttpGet("{username}")]
        public async Task<ActionResult<AccountInfoDto>> GetAccountInfoByUsername(string username)
            => Ok(await _accountService.GetAccountByUsername(username));

        [HttpPost]
        public async Task<ActionResult<CreateAccountDto>> CreateAccount(CreateAccountDto account)
        {
            try
            {
                var result = await _accountService.CreateAccount(account);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("infos/{username}")]
        public async Task<ActionResult<AccountInfoDto>> UpdateAccountInfo(string username, AccountInfoDto accountInfo)
            => Ok(await _accountService.UpdateAccountInfo(username, accountInfo));

        [HttpPatch("roles/{username}")]
        public async Task<ActionResult> UpdateAccountRole(string username, IEnumerable<string> roles)
            => await _accountService.UpdateAccountRole(username, roles) 
            ? Ok($"Update {username} roles successfuly") 
            : BadRequest($"Update {username} roles failed");

        [HttpPatch("password/{username}")]
        public async Task<ActionResult> UpdateAccountPassword(string username, string password)
            => Ok();
    }
}
