using System.ComponentModel.DataAnnotations.Schema;

namespace MotorcycleRepairShop.Domain.Entities
{
    public class Image : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public int? VehicleId { get; set; }
        [ForeignKey(nameof(VehicleId))]
        public Vehicle? Vehicle { get; set; }

        //// Nullable foreign key to ServiceRequest
        //public int? ServiceRequestId { get; set; }
        //public ServiceRequest? ServiceRequest { get; set; }
    }
}
