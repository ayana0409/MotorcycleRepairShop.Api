using MotorcycleRepairShop.Application.Model.Service;

namespace MotorcycleRepairShop.Application.Interfaces.Services
{
    public interface IHomeService
    {
        Task<IEnumerable<ServiceDto>> GetServiceList();
    }
}
