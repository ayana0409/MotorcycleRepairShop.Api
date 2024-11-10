namespace MotorcycleRepairShop.Application.Model
{
    public class PartHomeDto
    {
        public string Name { get; set; } = string.Empty;
        public int WarrantyPeriod { get; set; }
        public decimal Price { get; set; }
        public string BrandName { get; set; } = string.Empty;
    }
}
