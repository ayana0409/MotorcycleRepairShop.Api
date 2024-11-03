using MotorcycleRepairShop.Share.Validation;
using System.ComponentModel.DataAnnotations;

namespace MotorcycleRepairShop.Application.Model
{
    public class PaymentRequestDto
    {
        [Required]
        public int OrderId { get; set; }
        [Required]
        [GreaterThanZero]
        public decimal Amount { get; set; }
    }
}
