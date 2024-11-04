using MotorcycleRepairShop.Application.Model;

namespace MotorcycleRepairShop.Application.Interfaces.Services
{
    public interface IPaymentService
    {
        Task<PaymentDto> CreateCrashPayment(CreatePaymentDto paymentDto);
        string CreateVNPayPayment(CreatePaymentDto request);
        Task ProcessVNPayReturn(VNPayReturnDto vnpayReturnDto);
    }
}
