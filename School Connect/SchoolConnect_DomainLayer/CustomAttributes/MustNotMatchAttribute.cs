using System.ComponentModel.DataAnnotations;

namespace SchoolConnect_DomainLayer.CustomAttributes
{
    public class MustNotMatchAttribute : ValidationAttribute
    {
        private readonly string _memberToNotMatch;

        public MustNotMatchAttribute(string memberToNotMatch)
        {
            _memberToNotMatch = memberToNotMatch;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var comparisonProperty = validationContext.ObjectType.GetProperty(_memberToNotMatch);
            if (comparisonProperty == null) return new ValidationResult($"Property with the name {_memberToNotMatch} not found.");

            var comparisonValue = comparisonProperty.GetValue(validationContext.ObjectInstance);
            if (Equals(value, comparisonValue)) return new ValidationResult($"{validationContext.DisplayName} should not match {_memberToNotMatch}");
            
            return ValidationResult.Success;
        }
    }
}
