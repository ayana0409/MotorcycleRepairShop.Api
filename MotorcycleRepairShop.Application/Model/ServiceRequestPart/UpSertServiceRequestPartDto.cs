using System.ComponentModel.DataAnnotations;

namespace MotorcycleRepairShop.Application.Model
{
    public class UpSertServiceRequestPartDto
    {
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1;

        [Required]
        public int PartId { get; set; }
    }
}
