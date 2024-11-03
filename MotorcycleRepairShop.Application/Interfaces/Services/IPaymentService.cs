using MotorcycleRepairShop.Application.Model;

namespace MotorcycleRepairShop.Application.Interfaces.Services
{
    public interface IPaymentService
    {
        string CreateVNPayPayment(PaymentRequestDto request);
        Task ProcessVNPayReturn(VNPayReturnDto vnpayReturnDto);
    }
}
