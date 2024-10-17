using MotorcycleRepairShop.Application.Model;

namespace MotorcycleRepairShop.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> AuthenticateAsync(LoginDto user);
        Task<AuthResponse> ExternalLoginAsync(string email, string name);
    }
}
