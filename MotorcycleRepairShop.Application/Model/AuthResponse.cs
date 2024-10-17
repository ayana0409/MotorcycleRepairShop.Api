namespace MotorcycleRepairShop.Application.Model
{
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expire { get; set; }
    }
}
