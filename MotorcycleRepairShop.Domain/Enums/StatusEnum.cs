namespace MotorcycleRepairShop.Domain.Enums
{
    public enum StatusEnum
    {
        /// <summary>
        /// The request is new.
        /// </summary>
        New = 1,

        /// <summary>
        /// The request is under inspection.
        /// </summary>
        UnderInspection = 2,

        /// <summary>
        /// The request is waiting for payment.
        /// </summary>
        AwaitingPayment = 3,

        /// <summary>
        /// The request is being processed.
        /// </summary>
        Processing = 4,

        /// <summary>
        /// The request has been completed.
        /// </summary>
        Completed = 5,

        /// <summary>
        /// The request has been canceled.
        /// </summary>
        Canceled = 6
    }
}
