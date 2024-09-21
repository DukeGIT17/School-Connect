using System.ComponentModel.DataAnnotations;

namespace SchoolConnect_DomainLayer.CustomAttributes
{
    public class ValidRolesAttribute : ValidationAttribute
    {
        private string[] _roles;

        public ValidRolesAttribute(params string[] roles)
        {
            _roles = roles;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            try
            {
                foreach (var role in _roles)
                {
                    if (value!.ToString() == role)
                    {
                        return ValidationResult.Success;
                    }
                }

                throw new Exception($"The role {value} did not match any valid role.");
            }
            catch (Exception ex)
            {
                return new ValidationResult(ex.Message);
            }
        }
    }
}
