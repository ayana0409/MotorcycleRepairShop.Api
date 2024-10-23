using Microsoft.AspNetCore.Http;

namespace MotorcycleRepairShop.Application.Model
{
    public class CreateServiceRequestDto : ServiceRequestBaseDto
    {
        public IEnumerable<IFormFile> Videos { get; set; } = [];
        public IEnumerable<IFormFile> Images { get; set; } = [];
        public List<int> Problems { get; set; } = [];

    }
}
