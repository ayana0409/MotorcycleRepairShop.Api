namespace MotorcycleRepairShop.Application.Model
{
    public class ServiceRequestStatisticsDto
    {
        public string Date { get; set; } = string.Empty;
        public Dictionary<string, int> ServiceTypeCounts { get; set; } = [];
    }
}
