namespace MotorcycleRepairShop.Application.Model
{
    public class ServiceRequestInfoDto : ServiceRequestUserInfoDto
    {
        public decimal TotalPrice { get; set; }
        public string ServiceType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public IEnumerable<string> Videos { get; set; } = [];
        public IEnumerable<string> Images { get; set; } = [];
        public IEnumerable<string> Problems { get; set; } = [];
        public IEnumerable<ServiceRequestItemInfo> Services { get; set; } = [];
        public IEnumerable<ServiceRequestPartInfoDto> Parts { get; set; } = [];
    }
}
