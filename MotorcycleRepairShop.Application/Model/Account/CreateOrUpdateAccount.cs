using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MotorcycleRepairShop.Application.Model.Account
{
    public class CreateOrUpdateAccount
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string MobilePhone { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
        
        public List<string> UserRoles { get; set; } = [];
    }
}
