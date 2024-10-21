using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotorcycleRepairShop.Domain.Entities
{
    public class Part : BaseEntity
    {
        [Required]
        [StringLength(250)]
        public string Name { get; set; } = string.Empty;
        [Required]
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdateDate { get; set; }
        [Required]
        public int WarrantyPeriod { get; set; }
        [Required]
        public decimal Price { get; set; }
        public bool IsActive { get; set; } = true;

        [Required]
        public int BrandId { get; set; }
        [ForeignKey(nameof(BrandId))]
        public Brand? Brand { get; set; }
    }
}
