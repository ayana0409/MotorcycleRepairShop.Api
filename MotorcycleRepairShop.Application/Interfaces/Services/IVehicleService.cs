using MotorcycleRepairShop.Application.Model;

namespace MotorcycleRepairShop.Application.Interfaces.Services
{
    public interface IVehicleService
    {
        Task<int> CreateVehicle(CreateVehicleDto vehicleDto);
        Task<VehicleDto> GetVehicleById(int id);
    }
}
