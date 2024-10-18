using System.ComponentModel.DataAnnotations;

namespace MotorcycleRepairShop.Application.Model.Service
{
    public class ServiceDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public decimal Price { get; set; }
    }
}
