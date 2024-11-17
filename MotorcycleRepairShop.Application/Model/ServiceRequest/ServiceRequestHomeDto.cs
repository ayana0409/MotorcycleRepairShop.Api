using MotorcycleRepairShop.Domain.Enums;

namespace MotorcycleRepairShop.Application.Model
{
    public class ServiceRequestHomeDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string IssueDescripton { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }
}
