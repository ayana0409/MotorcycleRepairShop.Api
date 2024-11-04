using MotorcycleRepairShop.Share.Validation;
using System.ComponentModel.DataAnnotations;

namespace MotorcycleRepairShop.Application.Model
{
    public class CreatePaymentDto
    {
        [Required]
        public int ServiceRequestId { get; set; }
        [Required]
        [GreaterThanZero]
        public decimal Amount { get; set; }
    }
}
