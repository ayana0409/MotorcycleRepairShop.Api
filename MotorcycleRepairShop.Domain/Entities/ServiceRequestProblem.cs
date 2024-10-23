using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotorcycleRepairShop.Domain.Entities
{
    public class ServiceRequestProblem
    {
        public DateTime ReportedDate { get; set; } = DateTime.UtcNow;

        [Required]
        public int ServiceRequestId { get; set; }
        [ForeignKey(nameof(ServiceRequestId))]
        public ServiceRequest? ServiceRequest { get; set; }

        [Required]
        public int ProblemId { get; set; }
        [ForeignKey(nameof(ProblemId))]
        public Problem? Problem { get; set; }
    }
}
