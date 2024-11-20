using MotorcycleRepairShop.Application.Model;

namespace MotorcycleRepairShop.Application.Interfaces.Services
{
    public interface IReportService
    {
        Task<ServiceRequestInvoiceDto> GetServiceRequestInvoiceById(int serviceRequestId);
    }
}
