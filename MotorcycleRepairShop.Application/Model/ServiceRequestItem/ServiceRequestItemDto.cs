﻿namespace MotorcycleRepairShop.Application.Model
{
    public class ServiceRequestItemDto : UpSertServiceRequestItemDto
    {
        public decimal Price { get; set; }

        public int ServiceRequestId { get; set; }

        public decimal TotalPrice { get => Quantity * Price; }
    }
}
