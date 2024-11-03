namespace MotorcycleRepairShop.Share.Exceptions
{
    public class InvalidSignatureException : ApplicationException
    {
        public InvalidSignatureException() : base("Sai chữ ký xác thực") { }
    }

    public class PaymentFailedException : ApplicationException
    {
        public string OrderId { get; }
        public PaymentFailedException(string orderId) : base($"Thanh toán thất bại cho đơn hàng {orderId}")
        {
            OrderId = orderId;
        }
    }
}
