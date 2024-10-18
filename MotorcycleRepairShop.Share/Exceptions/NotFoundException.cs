namespace MotorcycleRepairShop.Share.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string entityName, object key)
            : base($"{entityName} with ID {key} was not found.")
        { }
    }
}
