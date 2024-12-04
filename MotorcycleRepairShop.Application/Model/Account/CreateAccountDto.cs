using MotorcycleRepairShop.Share.Validation;
using System.ComponentModel.DataAnnotations;

namespace MotorcycleRepairShop.Application.Model.Account
{
    public class CreateAccountDto : AccountInfoDto
    {

        [Required]
        [PasswordValidation]
        public string Password { get; set; } = string.Empty;
        public List<string> UserRoles { get; set; } = [];
    }
}
