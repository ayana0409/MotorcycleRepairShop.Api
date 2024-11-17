using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MotorcycleRepairShop.Application.Model
{
    public class UpdateVehicleDto : VehicleBaseDto
    {
        public List<string> ImageUrls { get; set; } = [];
        public List<IFormFile> Images { get; set; } = [];

        [Required]
        public int BrandId { get; set; }
    }
}
