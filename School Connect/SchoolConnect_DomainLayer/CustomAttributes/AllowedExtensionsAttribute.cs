using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SchoolConnect_DomainLayer.CustomAttributes
{
    public class AllowedExtensionsAttribute(params string[] extensions) : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            try
            {
                if (value is null)
                    return ValidationResult.Success;

                var file = value as IFormFile;
                if (file is not null)
                {
                    var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                    foreach (var extension in extensions)
                    {
                        if (fileExtension == extension)
                        {
                            return ValidationResult.Success;
                        }
                    }
                    return new ValidationResult($"A file of type {fileExtension} is not allowed, please provide a file with a valid extension. E.g., {extensions.First()}");
                }

                return new ValidationResult($"Property {validationContext.MemberName} is not of type IFormFile");
            }
            catch (Exception ex)
            {
                return new ValidationResult(ex.Message);
            }
        }
    }
}
