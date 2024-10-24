using System.ComponentModel.DataAnnotations;

namespace MotorcycleRepairShop.Application.Model
{
    public class UpsSertServiceRequestItemDto
    {
        public int Quantity { get; set; }

        [Required]
        public int ServiceId { get; set; }
    }
}
