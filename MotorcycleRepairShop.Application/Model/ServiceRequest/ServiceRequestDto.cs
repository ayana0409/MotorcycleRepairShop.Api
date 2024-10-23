namespace MotorcycleRepairShop.Application.Model
{
    public class ServiceRequestDto : ServiceRequestBaseDto
    {
        public string Status { get; set; } = string.Empty;
        public IEnumerable<string> Videos { get; set; } = [];
        public IEnumerable<string> Images { get; set; } = [];
        public IEnumerable<string> Problems { get; set; } = [];
    }
}
