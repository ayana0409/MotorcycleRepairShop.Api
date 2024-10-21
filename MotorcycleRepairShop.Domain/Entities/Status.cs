using System.ComponentModel.DataAnnotations;

namespace MotorcycleRepairShop.Domain.Entities
{
    public class Status : BaseEntity
    {
        [Required]
        [StringLength(250)]
        public string StatusName { get; set; } = string.Empty;
        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
    }
}
