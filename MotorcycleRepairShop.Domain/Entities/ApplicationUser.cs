using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MotorcycleRepairShop.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [StringLength(15)]
        public string MobilePhone { get; set; } = string.Empty;

        [Required]
        [StringLength(250)]
        public string Address { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; } = [];
        public virtual ICollection<ServiceRequest> ServiceRequests { get; set; } = [];
    }
}
