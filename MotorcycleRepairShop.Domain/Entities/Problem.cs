using System.ComponentModel.DataAnnotations;

namespace MotorcycleRepairShop.Domain.Entities
{
    public class Problem : BaseEntity
    {
        [Required]
        [StringLength(250)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public virtual ICollection<ServiceRequestProblem> ServiceRequestProblems { get; set; } = [];
    }
}
