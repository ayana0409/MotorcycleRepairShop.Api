using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MotorcycleRepairShop.Application.Configurations;
using MotorcycleRepairShop.Application.Configurations.Models;
using MotorcycleRepairShop.Application.Interfaces.Services;
using MotorcycleRepairShop.Application.Model;
using MotorcycleRepairShop.Share.Exceptions;
using MotorcycleRepairShop.Share.Extensions;
using Serilog;

namespace MotorcycleRepairShop.Infrastructure.Services
{
    public class PaymentService : BaseService, IPaymentService
    {
        private readonly VNPayConfig _vnpayConfig;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PaymentService(IOptions<VNPayConfig> vnpayConfig, IHttpContextAccessor httpContextAccessor, ILogger logger) : base(logger)
        {
            _vnpayConfig = vnpayConfig.Value;
            _httpContextAccessor = httpContextAccessor;
        }

        #region Direct



        #endregion

        #region VNPay

        public string CreateVNPayPayment(PaymentRequestDto request)
        {
            var vnp_TmnCode = _vnpayConfig.TmnCode;
            var vnp_HashSecret = _vnpayConfig.HashSecret;
            var vnp_Url = _vnpayConfig.Url;
            var vnp_ReturnUrl = _vnpayConfig.ReturnUrl;

            var ipAddr = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString()
                ?? throw new ApplicationException("PaymentService - CreateVNPayPayment - ipAddr");
            var vnPayData = new VNPayLibrary();

            vnPayData.AddRequestData("vnp_Version", "2.1.0");
            vnPayData.AddRequestData("vnp_Command", "pay");
            vnPayData.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnPayData.AddRequestData("vnp_Amount", (request.Amount * 100).ToString());
            vnPayData.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnPayData.AddRequestData("vnp_CurrCode", "VND");
            vnPayData.AddRequestData("vnp_IpAddr", ipAddr);
            vnPayData.AddRequestData("vnp_Locale", "vn");
            vnPayData.AddRequestData("vnp_OrderInfo", $"Thanh toan don hang {request.OrderId}");
            vnPayData.AddRequestData("vnp_OrderType", "billpayment");
            vnPayData.AddRequestData("vnp_ReturnUrl", vnp_ReturnUrl);
            vnPayData.AddRequestData("vnp_TxnRef", request.OrderId.ToString());

            var result = vnPayData.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            _logger.Information($"Create payment successful: {request.OrderId}");
            return result;
        }

        public async Task ProcessVNPayReturn(VNPayReturnDto vnpayReturnDto)
        {
            var vnp_HashSecret = _vnpayConfig.HashSecret;
            var vnPayData = new VNPayLibrary();

            foreach (var (key, value) in vnpayReturnDto.GetType().GetProperties()
                .Where(p => p.GetValue(vnpayReturnDto) != null)
                .ToDictionary(p => p.Name, p => p.GetValue(vnpayReturnDto)?.ToString()))
            {
                vnPayData.AddResponseData(key, value);
            }

            if (!vnPayData.ValidateSignature(vnp_HashSecret))
                throw new InvalidSignatureException();

            if (vnpayReturnDto.Vnp_ResponseCode != "00")
                throw new PaymentFailedException(vnpayReturnDto.Vnp_TxnRef);
        }
        #endregion
    }
}
