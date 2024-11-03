namespace MotorcycleRepairShop.Application.Model
{
    public class VNPayReturnDto
    {
        public string Vnp_TxnRef { get; set; } = string.Empty;
        public string Vnp_ResponseCode { get; set; } = string.Empty;
        public string Vnp_SecureHash { get; set; } = string.Empty;
    }
}
