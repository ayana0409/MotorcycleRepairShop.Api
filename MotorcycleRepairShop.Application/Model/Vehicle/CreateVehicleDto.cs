using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MotorcycleRepairShop.Application.Model
{
    public class CreateVehicleDto : VehicleBaseDto
    {
        public List<IFormFile> Images { get; set; } = [];

        [Required]
        public int BrandId { get; set; }
    }
}
