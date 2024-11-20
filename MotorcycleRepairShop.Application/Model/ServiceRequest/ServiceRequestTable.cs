using MotorcycleRepairShop.Domain.Enums;

namespace MotorcycleRepairShop.Application.Model
{
    public class ServiceRequestTable : ServiceRequestUserInfoDto
    {
        public int Id { get; set; }
        public decimal TotalPrice { get; set; }
        public string ServiceType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
