using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MotorcycleRepairShop.Application.Configurations.Models;
using MotorcycleRepairShop.Application.Interfaces;
using MotorcycleRepairShop.Application.Interfaces.Services;
using MotorcycleRepairShop.Application.Model;
using MotorcycleRepairShop.Domain.Entities;
using MotorcycleRepairShop.Domain.Enums;
using MotorcycleRepairShop.Share.Exceptions;
using MotorcycleRepairShop.Share.Extensions;
using Serilog;

namespace MotorcycleRepairShop.Infrastructure.Services
{
    public class PaymentService : BaseService, IPaymentService
    {
        private readonly VNPayConfig _vnpayConfig;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymentService(IOptions<VNPayConfig> vnpayConfig, IHttpContextAccessor httpContextAccessor, ILogger logger, IUnitOfWork unitOfWork, IMapper mapper) : base(logger)
        {
            _vnpayConfig = vnpayConfig.Value;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Crash

        public async Task<PaymentDto> CreateCrashPayment(CreatePaymentDto paymentDto)
            => await CreatePayment(paymentDto, PaymentMethodEnum.Cash);

        private async Task<PaymentDto> CreatePayment(CreatePaymentDto paymentDto, PaymentMethodEnum paymentMethod)
        {
            var serviceRequestId = paymentDto.ServiceRequestId;
            var existServiceRequest = await _unitOfWork.ServiceRequestRepository
                .GetSigleAsync(s => s.Id.Equals(serviceRequestId))
                ?? throw new NotFoundException(nameof(ServiceRequest), serviceRequestId);

            var payment = _mapper.Map<Payment>(paymentDto);
            payment.PaymentMethod = paymentMethod;
            await _unitOfWork.PaymentRepository.CreateAsync(payment);

            existServiceRequest.StatusId = (int)StatusEnum.Processing;
            _unitOfWork.ServiceRequestRepository
                .Update(existServiceRequest);
            await _unitOfWork.SaveChangeAsync();

            _logger.Information($"Create Payment successful - ServiceRequestId: {serviceRequestId}" +
                $" - Amount: {paymentDto.Amount} - Method: {paymentMethod.GetDisplayName()}");
            var result = _mapper.Map<PaymentDto>(payment);
            return result;
        }

        #endregion

        #region VNPay

        public string CreateVNPayPayment(CreatePaymentDto request)
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
            vnPayData.AddRequestData("vnp_OrderInfo", $"Thanh toan don hang {request.ServiceRequestId}");
            vnPayData.AddRequestData("vnp_OrderType", "billpayment");
            vnPayData.AddRequestData("vnp_ReturnUrl", vnp_ReturnUrl);
            vnPayData.AddRequestData("vnp_TxnRef", request.ServiceRequestId.ToString());

            var result = vnPayData.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            _logger.Information($"Create payment successful: {request.ServiceRequestId}");
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
