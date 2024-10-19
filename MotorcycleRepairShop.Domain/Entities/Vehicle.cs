using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotorcycleRepairShop.Domain.Entities
{
    public class Vehicle : BaseEntity
    {
        [Required]
        [StringLength(250)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [StringLength(250)]
        public string Version { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        [Required]
        public int BrandId { get; set; }
        [ForeignKey(nameof(BrandId))]
        public Brand? Brand { get; set; }

        public virtual ICollection<Image> Images { get; set; } = [];
    }
}
