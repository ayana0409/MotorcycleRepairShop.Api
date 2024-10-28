namespace MotorcycleRepairShop.Application.Model
{
    public class ServiceRequestPartDto : UpSertServiceRequestPartDto
    {
        public decimal Price { get; set; }

        public int ServiceRequestId { get; set; }

        public decimal TotalPrice { get => Quantity * Price; }
    }
}
