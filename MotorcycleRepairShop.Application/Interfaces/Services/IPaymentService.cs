using MotorcycleRepairShop.Application.Model;

namespace MotorcycleRepairShop.Application.Interfaces.Services
{
    public interface IPaymentService
    {
        Task<bool> CapturePayPalOrder(string orderId, int serviceRequestId);
        Task<PaymentDto> CreateCrashPayment(CreatePaymentDto paymentDto);
        Task<string> CreatePayPalOrder(CreatePaymentDto paymentDto);
        string CreateVNPayPayment(CreatePaymentDto request);
        Task ProcessVNPayReturn(VNPayReturnDto vnpayReturnDto);
    }
}
