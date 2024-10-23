using System.ComponentModel.DataAnnotations;

namespace MotorcycleRepairShop.Domain.Entities
{
    public class Video : BaseEntity
    {
        [Required]
        [MaxLength(500)]
        public string Name { get; set; } = string.Empty;
        public bool SubmittedByStaff { get; set; } = false;

        [Required]
        public int ServiceRequestId { get; set; }
        public ServiceRequest? ServiceRequest { get; set; }
    }
}
