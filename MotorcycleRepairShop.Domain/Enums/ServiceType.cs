using System.ComponentModel.DataAnnotations;

namespace MotorcycleRepairShop.Domain.Enums
{
    public enum ServiceType
    {
        [Display(Name = "Cứu hộ")]
        Rescue,
        [Display(Name = "Trực tiếp")]
        Direct,
        [Display(Name = "Từ xa")]
        Remote
    }
}
