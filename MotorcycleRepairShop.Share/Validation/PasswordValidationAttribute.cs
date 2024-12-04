using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MotorcycleRepairShop.Share.Validation
{
    public class PasswordValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not string password)
            {
                return new ValidationResult("Invalid Password.");
            }

            var hasMinimum8Chars = password.Length >= 8;
            var hasUpperCase = Regex.IsMatch(password, @"[A-Z]");
            var hasLowerCase = Regex.IsMatch(password, @"[a-z]");
            var hasNumber = Regex.IsMatch(password, @"\d");
            var hasSpecialChar = Regex.IsMatch(password, @"[!@#$%^&*(),.?""{}|<>]");

            if (!hasMinimum8Chars || !hasUpperCase || !hasLowerCase || !hasNumber || !hasSpecialChar)
            {
                return new ValidationResult("Passwords must be at least 8 characters, including uppercase, lowercase, numbers, and special characters.");
            }

            return ValidationResult.Success!;
        }
    }
}
