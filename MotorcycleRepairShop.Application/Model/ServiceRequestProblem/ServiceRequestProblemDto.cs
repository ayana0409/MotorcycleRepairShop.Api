namespace MotorcycleRepairShop.Application.Model
{
    public class ServiceRequestProblemDto
    {
        public int ProblemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ServiceRequestId { get; set; }
        public DateTime ReportedDate { get; set; }
    }
}
