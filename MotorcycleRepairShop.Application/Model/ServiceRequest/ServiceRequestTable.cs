namespace MotorcycleRepairShop.Application.Model
{
    public class ServiceRequestTable : ServiceRequestUserInfoDto
    {
        public int Id { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
