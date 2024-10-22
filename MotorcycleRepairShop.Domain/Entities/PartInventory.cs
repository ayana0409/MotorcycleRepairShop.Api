using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotorcycleRepairShop.Domain.Entities
{
    public class PartInventory : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string BatchNumber { get; set; } = string.Empty;
        [Required]
        [StringLength(250)]
        public string Supplier { get; set; } = string.Empty;
        [Required]
        [Range(0, 100)]
        public decimal Tax { get; set; }
        [Required]
        public DateTime EntryDate { get; set; } = DateTime.UtcNow;
        [Required]
        public int QuantityReceived { get; set; }
        [Required]
        public int QuantityInStock { get; set; }
        [Required]
        public decimal EntryPrice { get; set; }

        [Required]
        public DateTime ProductionDate { get; set; }
        [Required]
        public DateTime ExpirationDate { get; set; }

        [Required]
        public int PartId { get; set; }
        [ForeignKey(nameof(PartId))]
        public Part? Part { get; set; }
    }
}
