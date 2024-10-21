using System.ComponentModel.DataAnnotations;

namespace MotorcycleRepairShop.Application.Model
{
    public class PartDto
    {
        [Required]
        [StringLength(250)]
        public string Name { get; set; } = string.Empty;
        [Required]
        public int WarrantyPeriod { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int BrandId { get; set; }
    }
}
