using System.ComponentModel.DataAnnotations;

namespace MotorcycleRepairShop.Application.Model
{
    public class UpsSertServiceRequestItemDto
    {
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1;

        [Required]
        public int ServiceId { get; set; }
    }
}
