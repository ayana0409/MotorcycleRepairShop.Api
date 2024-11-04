using MotorcycleRepairShop.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotorcycleRepairShop.Domain.Entities
{
    public class Payment : BaseEntity
    {
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public string? TransactionId { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "varchar(50)")]
        public PaymentMethodEnum PaymentMethod { get; set; } = PaymentMethodEnum.Cash;

        public bool IsSuccessful { get; set; } = true;
        [Column(TypeName = "varchar(500)")]
        public string? Note { get; set; }

        [Required]
        public int ServiceRequestId { get; set; }
        [ForeignKey(nameof(ServiceRequestId))]
        public ServiceRequest? ServiceRequest { get; set; }
    }
}
