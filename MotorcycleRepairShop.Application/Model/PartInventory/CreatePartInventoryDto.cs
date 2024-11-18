using System.ComponentModel.DataAnnotations;

namespace MotorcycleRepairShop.Application.Model
{
    public class CreatePartInventoryDto
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
        public int QuantityReceived { get; set; }
        [Required]
        public decimal EntryPrice { get; set; }

        [Required]
        public DateTime ProductionDate { get; set; }
        [Required]
        public DateTime ExpirationDate { get; set; }

        [Required]
        public int PartId { get; set; }
    }
}
