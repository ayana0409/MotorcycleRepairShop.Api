namespace MotorcycleRepairShop.Application.Model
{
    public class ServiceRequestPartInfoDto
    {
        public string Name { get; set; } = string.Empty;
        public DateTime? WarrantyTo { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
