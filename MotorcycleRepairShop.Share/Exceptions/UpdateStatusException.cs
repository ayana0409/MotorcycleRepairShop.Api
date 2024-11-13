namespace MotorcycleRepairShop.Share.Exceptions
{
    public class UpdateStatusFailedException : ApplicationException
    {
        public UpdateStatusFailedException(string from, string to) 
            : base($"Can't update status from ${from} to ${to}.")
        {  
        }
    }
}
