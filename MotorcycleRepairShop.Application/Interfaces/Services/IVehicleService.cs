using MotorcycleRepairShop.Application.Model;

namespace MotorcycleRepairShop.Application.Interfaces.Services
{
    public interface IVehicleService
    {
        Task<int> CreateVehicle(CreateVehicleDto vehicleDto);
        Task DeleteVehicle(int vehicleId);
        Task<VehicleDto> GetVehicleById(int id);
        Task<TableResponse<VehicleTableDto>> GetVehiclePagination(TableRequest request);
        Task<VehicleDto> UpdateVehicle(int vehicleId, UpdateVehicleDto vehicleDto);
    }
}
