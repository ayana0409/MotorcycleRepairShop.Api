using System.ComponentModel.DataAnnotations;

namespace MotorcycleRepairShop.Application.Model
{
    public class VehicleBaseDto
    {
        [Required]
        [StringLength(250)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [StringLength(250)]
        public string Version { get; set; } = string.Empty;

        [Required]
        public int BrandId { get; set; }
    }
}
