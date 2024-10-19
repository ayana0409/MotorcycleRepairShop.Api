using Microsoft.AspNetCore.Http;

namespace MotorcycleRepairShop.Application.Model
{
    public class CreateVehicleDto : VehicleBaseDto
    {
        public List<IFormFile> Images { get; set; } = [];
    }
}
