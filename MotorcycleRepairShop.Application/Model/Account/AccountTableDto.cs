using MotorcycleRepairShop.Application.Model.Account;

namespace MotorcycleRepairShop.Application.Model
{
    public class AccountTableDto : AccountInfoDto
    {
        public string Username { get; set; } = string.Empty;
        public IList<string> UserRoles { get; set; } = [];
    }
}
