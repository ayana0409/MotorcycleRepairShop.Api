using MotorcycleRepairShop.Application.Model;
using MotorcycleRepairShop.Application.Model.Service;

namespace MotorcycleRepairShop.Application.Interfaces.Services
{
    public interface IHomeService
    {
        Task<IEnumerable<PartHomeDto>> GetPartList();
        Task<IEnumerable<ServiceHomeDto>> GetServiceList();
    }
}
