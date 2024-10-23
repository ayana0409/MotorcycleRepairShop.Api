using System.ComponentModel.DataAnnotations.Schema;

namespace MotorcycleRepairShop.Domain.Entities
{
    public class Image : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public int? VehicleId { get; set; }
        [ForeignKey(nameof(VehicleId))]
        public Vehicle? Vehicle { get; set; }

        public int? ServiceRequestId { get; set; }
        [ForeignKey(nameof(ServiceRequestId))]
        public ServiceRequest? ServiceRequest { get; set; }
    }
}
