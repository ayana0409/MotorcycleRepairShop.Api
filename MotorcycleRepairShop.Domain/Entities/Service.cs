using System.ComponentModel.DataAnnotations;

namespace MotorcycleRepairShop.Domain.Entities
{
    public class Service : BaseEntity
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public decimal Price { get; set; }
        [Required]
        public bool IsActive { get; set; } = true;
    }
}
