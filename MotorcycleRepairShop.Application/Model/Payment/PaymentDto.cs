using MotorcycleRepairShop.Domain.Enums;

namespace MotorcycleRepairShop.Application.Model
{
    public class PaymentDto : CreatePaymentDto
    {
        public DateTime PaymentDate { get; set; }
        public string? TransactionId { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
    }
}
