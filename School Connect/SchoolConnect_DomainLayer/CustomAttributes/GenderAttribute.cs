using System.ComponentModel.DataAnnotations;

namespace SchoolConnect_DomainLayer.CustomAttributes
{
    public class GenderAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not string stringValue)
            {
                return new ValidationResult($"{value} could not be converted to a text");
            }

            if (stringValue.Equals("MALE", StringComparison.CurrentCultureIgnoreCase) || stringValue.Equals("FEMALE", StringComparison.CurrentCultureIgnoreCase))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult($"Please enter a valid gender");
        }
    }
}
