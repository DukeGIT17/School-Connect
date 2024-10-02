

using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace SchoolConnect_DomainLayer.CustomAttributes
{
    public class MaxItemsAttribute(int maxItems) : ValidationAttribute
    {
        private int _maxItems = maxItems;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IEnumerable enumerable)
            {
                int itemCount = enumerable.Cast<object>().Count();
                if (itemCount > _maxItems)
                    return new ValidationResult($"The collection {validationContext.DisplayName} cannot have more than {_maxItems} items.");

                return ValidationResult.Success;
            }
            return new ValidationResult($"The property {validationContext.DisplayName} is not a valid collection.");
        }
    }
}
