namespace MotorcycleRepairShop.Application.Model
{
    public class ServiceRequestDto : ServiceRequestUserInfoDto
    {
        public string ServiceType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public IEnumerable<string> Videos { get; set; } = [];
        public IEnumerable<string> Images { get; set; } = [];
        public IEnumerable<string> Problems { get; set; } = [];
        public IEnumerable<string> Services { get; set; } = [];
        public IEnumerable<string> Parts { get; set; } = [];
    }
}
