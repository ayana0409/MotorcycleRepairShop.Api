using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MotorcycleRepairShop.Domain.Enums
{
    public enum StatusEnum
    {
        /// <summary>
        /// The request is new.
        /// </summary>
        [Display(Name = "Mới")]
        New = 1,

        /// <summary>
        /// The request is under inspection.
        /// </summary>
        [Display(Name = "Đang kiểm tra")]
        UnderInspection = 2,

        /// <summary>
        /// The request is waiting for payment.
        /// </summary>
        [Display(Name = "Đang đợi thanh toán")]
        AwaitingPayment = 3,

        /// <summary>
        /// The request is being processed.
        /// </summary>
        [Display(Name = "Đang xử lý")]
        Processing = 4,

        /// <summary>
        /// The request has been completed.
        /// </summary>
        [Display(Name = "Hoàn thành")]
        Completed = 5,

        /// <summary>
        /// The request has been canceled.
        /// </summary>
        [Display(Name = "Hủy")]
        Canceled = 6
    }
}
