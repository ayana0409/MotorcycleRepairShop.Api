namespace MotorcycleRepairShop.Application.Model
{
    public class VehicleTableDto : VehicleDto
    {
        public int Id { get; set; }
        public string BrandName { get; set; } = string.Empty;
    }
}
