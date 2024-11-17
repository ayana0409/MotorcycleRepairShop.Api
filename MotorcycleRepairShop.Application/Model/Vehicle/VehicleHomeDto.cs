namespace MotorcycleRepairShop.Application.Model
{
    public class VehicleHomeDto : VehicleBaseDto
    {
        public IEnumerable<string> Images { get; set; } = [];
        public string BrandName { get; set; } = string.Empty;
    }
}
