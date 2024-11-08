namespace MotorcycleRepairShop.Application.Model
{
    public class PartTableDto : PartDto
    {
        public int Id { get; set; }
        public string BrandName { get; set; } = string.Empty;
    }
}
