using System.ComponentModel.DataAnnotations;

namespace MotorcycleRepairShop.Application.Model.Account
{
    public class CreateAccountDto : AccountInfoDto
    {

        [Required]
        public string Password { get; set; } = string.Empty;
        public List<string> UserRoles { get; set; } = [];
    }
}
