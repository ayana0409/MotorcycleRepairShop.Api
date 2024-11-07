using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MotorcycleRepairShop.Application.Model.Account
{
    public class AccountInfoDto
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string MobilePhone { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;
        
    }
}
