using Microsoft.AspNetCore.Mvc;
using MotorcycleRepairShop.Application.Interfaces.Services;
using MotorcycleRepairShop.Application.Model;

namespace MotorcycleRepairShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("service-request/{id}")]
        public async Task<ActionResult<IEnumerable<PaymentDto>>> GetByServiceRequestId(int id)
            => Ok(await _paymentService.GetByServiceRequestId(id));

        #region Cash
        /// <summary>
        /// Tạo thanh toán bằng tiền mặt
        /// </summary>
        /// <param name="paymentDto">Thông tin cần thanh toán</param>
        /// <returns></returns>
        [HttpPost("cash")]
        public async Task<ActionResult<PaymentDto>> CreateCrashPayment(CreatePaymentDto paymentDto)
            => CreatedAtAction(nameof(CreateCrashPayment), await _paymentService.CreateCrashPayment(paymentDto));

        #endregion

        #region PayPal

        [HttpPost("PayPal/create")]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentDto paymentDto)
            => Ok(new { Url = await _paymentService.CreatePayPalOrder(paymentDto) });

        [HttpGet("PayPal/execute-payment")]
        public async Task<ActionResult> ExecutePayment(string token, int serviceRequestId)
            => await _paymentService.CapturePayPalOrder(token, serviceRequestId) 
            ? Ok("Thanh toán thành công, bạn có thể đóng trang này.") 
            : BadRequest("Payment could not be completed.");

        [HttpGet("PayPal/cancel-payment")]
        public IActionResult CancelPayment()
            => BadRequest("Payment was canceled by the user.");

        [HttpGet("paypal/check-payment/{token}")]
        public async Task<IActionResult> CheckPayPalPayment(string token)
            => await _paymentService.CheckPayPalPayment(token) ? Ok() : BadRequest();
        #endregion

        #region VNPay - Not Working

        //[HttpPost("VNPay/create")]
        //public ActionResult<string> CreatePayment([FromBody] PaymentRequestDto request)
        //    => Ok(_paymentService.CreateVNPayPayment(request));

        //[HttpPost("VNPay/return")]
        //public async Task<ActionResult> VNPayReturn([FromBody] VNPayReturnDto vnpayReturnDto)
        //{
        //    await _paymentService.ProcessVNPayReturn(vnpayReturnDto);
        //    return NoContent();
        //}

        #endregion
    }
}
