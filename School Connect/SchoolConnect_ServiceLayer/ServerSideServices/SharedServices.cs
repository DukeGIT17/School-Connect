

using System.ComponentModel.DataAnnotations;

namespace SchoolConnect_ServiceLayer.ServerSideServices
{
    public static class SharedValidationService
    {
        public static Dictionary<string, object> AttemptObjectValidation<T>(T obj)
        {
            try
            {
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(obj!, serviceProvider: null, items: null);
                bool isValid = Validator.TryValidateObject(obj!, validationContext, validationResults);
                List<string>? errors = [];

                if (!isValid)
                {
                    foreach (var validationResult in validationResults)
                        errors.Add(validationResult.ErrorMessage
                            ?? "Something went wrong: Check the System Admin Service in the ServiceLayer.");
                    return new Dictionary<string, object>
                    {
                        { "Success", false },
                        { "Errors", errors }
                    };
                }

                return new Dictionary<string, object>
                {
                    { "Success", true }
                };
            }
            catch (Exception ex)
            {
                return new Dictionary<string, object>
                {
                    {"Success", false },
                    {"ErrorMessage", ex.Message }
                };
            }
        }
    }
}
