using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MotorcycleRepairShop.Domain.Entities
{
    public class ServiceRequestPart
    {
        public DateTime? WarrantyTo {  get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        [Required]
        public int ServiceRequestId { get; set; }
        [ForeignKey(nameof(ServiceRequestId))]
        public ServiceRequest? ServiceRequest { get; set; }

        [Required]
        public int PartId { get; set; }
        [ForeignKey(nameof(PartId))]
        public Part? Part { get; set; }

        public decimal GetTotalPrice()
            => Quantity * Price;
    }
}
