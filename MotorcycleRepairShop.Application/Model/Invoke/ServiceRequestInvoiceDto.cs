using MotorcycleRepairShop.Domain.Enums;

namespace MotorcycleRepairShop.Application.Model
{
    public class ServiceRequestInvoiceDto
    {
        public int Id { get; set; }
        public string MobilePhone { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? IssueDescription { get; set; }
        public string DateSubmitted { get; set; } = string.Empty;
        public string CompletionDate { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public ServiceType ServiceType { get; set; } = ServiceType.Direct;
        public IEnumerable<string> Problems { get; set; } = [];
        public IEnumerable<ServiceRequestItemInfo> Services { get; set; } = [];
        public IEnumerable<ServiceRequestPartInfoDto> Parts { get; set; } = [];
    }
}
