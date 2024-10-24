using System.ComponentModel.DataAnnotations;

namespace MotorcycleRepairShop.Application.Model
{
    public class CreateServiceRequestItemDto
    {
        public int Quantity { get; set; }

        [Required]
        public int ServiceId { get; set; }
    }
}
