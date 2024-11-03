using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MotorcycleRepairShop.Application.Interfaces.Services;
using MotorcycleRepairShop.Application.Model;
using MotorcycleRepairShop.Share.Exceptions;

namespace MotorcycleRepairShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        #region Direct



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
