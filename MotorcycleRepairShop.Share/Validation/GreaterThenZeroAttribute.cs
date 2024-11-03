using System.ComponentModel.DataAnnotations;

namespace MotorcycleRepairShop.Share.Validation
{
    public class GreaterThanZeroAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is decimal decimalValue && decimalValue <= 0)
            {
                return new ValidationResult("Amount must be greater than zero.");
            }

            return ValidationResult.Success;
        }
    }
}
