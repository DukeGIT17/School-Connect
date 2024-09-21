using System.ComponentModel.DataAnnotations;

namespace SchoolConnect_DomainLayer.CustomAttributes
{
    public class MustMatchAttribute : ValidationAttribute
    {
        private readonly string _memberToMatch;
        
        public MustMatchAttribute(string memberToMatch)
        {
            this._memberToMatch = memberToMatch;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var comparisonProperty = validationContext.ObjectType.GetProperty(_memberToMatch);
            if (comparisonProperty == null) return new ValidationResult($"Property with the name {_memberToMatch} not found.");

            var comparisonvalue = comparisonProperty.GetValue(validationContext.ObjectInstance);
            if (!Equals(value, comparisonvalue)) return new ValidationResult($"{validationContext.DisplayName} must match {_memberToMatch}.");
            
            return ValidationResult.Success;
        }
    }
}
