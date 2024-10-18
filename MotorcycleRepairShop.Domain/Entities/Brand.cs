using System.ComponentModel.DataAnnotations;

namespace MotorcycleRepairShop.Domain.Entities
{
    public class Brand : BaseEntity
    {
        [Required]
        [StringLength(250)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [StringLength(100)]
        public string Country { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
