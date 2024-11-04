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

        #region Crash
        /// <summary>
        /// Tạo thanh toán bằng tiền mặt
        /// </summary>
        /// <param name="paymentDto">Thông tin cần thanh toán</param>
        /// <returns></returns>
        [HttpPost("crash")]
        public async Task<ActionResult<PaymentDto>> CreateCrashPayment(CreatePaymentDto paymentDto)
            => CreatedAtAction(nameof(CreateCrashPayment), await _paymentService.CreateCrashPayment(paymentDto));

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
