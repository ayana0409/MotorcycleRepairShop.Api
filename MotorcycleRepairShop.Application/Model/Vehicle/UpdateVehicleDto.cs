using Microsoft.AspNetCore.Http;

namespace MotorcycleRepairShop.Application.Model
{
    public class UpdateVehicleDto : VehicleBaseDto
    {
        public List<string> ImageUrls { get; set; } = [];
        public List<IFormFile> Images { get; set; } = [];
    }
}
