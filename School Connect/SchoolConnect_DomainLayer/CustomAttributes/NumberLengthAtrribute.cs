using System.ComponentModel.DataAnnotations;

namespace SchoolConnect_DomainLayer.CustomAttributes
{
    public class NumberLengthAttribute : ValidationAttribute
    {
        private readonly int _expectedLength;

        public NumberLengthAttribute(int expectedLength)
        {
            _expectedLength = expectedLength;
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                if (value.ToString()!.Length == _expectedLength)
                {
                    return ValidationResult.Success!;
                }
            }

            return new ValidationResult($"The {validationContext.DisplayName} field must be exactly {_expectedLength} digits. ({value})");
        }
    }
}
