using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MotorcycleRepairShop.Application.Interfaces.Services;
using MotorcycleRepairShop.Application.Model;
using MotorcycleRepairShop.Domain.Entities;
using Serilog;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace MotorcycleRepairShop.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            ILogger logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<AuthResponse> AuthenticateAsync(LoginDto user)
        {
            var loginUser = await _userManager.FindByNameAsync(user.UserName)
                ?? throw new KeyNotFoundException(message: "Invalid username or password.");

            _logger.Information($"START - Authentication: {user.UserName}");

            var result = await _signInManager.CheckPasswordSignInAsync(loginUser, user.Password, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                _logger.Information($"END - Authentication: {user.UserName} Unauthorize");
                throw new KeyNotFoundException(message: "Invalid username or password.");
            }

            _logger.Information($"END - Authentication: {user.UserName}");
            return new AuthResponse
            {
                Token = await GenerateJwtToken(loginUser),
                Expire = DateTime.UtcNow.AddDays(int.Parse(_configuration["Jwt:TokenLifetime"]))
            };
        }

        public Task<AuthResponse> ExternalLoginAsync(string email, string name)
        {
            throw new NotImplementedException();
        }

        //public async Task<AuthResponse> ExternalLoginAsync(string email, string name)
        //{
        //    if (string.IsNullOrEmpty(email))
        //    {
        //        throw new ApplicationException("Email must not be empty.");
        //    }

        //    var loginUser = await _userManager.FindByNameAsync(email);
        //    if (loginUser == null)
        //    {
        //        int firstTypeId = _unitOfWork.Table<UserType>()
        //                                        .OrderBy(t => t.Point)
        //                                        .FirstOrDefaultAsync().Id;
        //        var newUser = new ApplicationUser
        //        {
        //            UserName = email,
        //            Email = email,
        //            FullName = name,
        //            TypeId = firstTypeId,
        //            IsActive = true,
        //        };

        //        var createResult = await _userManager.CreateAsync(newUser);
        //        if (!createResult.Succeeded)
        //        {
        //            throw new ApplicationException("User creation failed.");
        //        }

        //        loginUser = newUser;
        //    }

        //    var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(loginUser);
        //    await _signInManager.Context.SignInAsync(IdentityConstants.ApplicationScheme, claimsPrincipal);

        //    return new AuthResponse
        //    {
        //        Token = await GenerateJwtToken(loginUser)
        //    };
        //}

        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.NameIdentifier, user.Id)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(int.Parse(_configuration["JWT:TokenLifetime"])),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
