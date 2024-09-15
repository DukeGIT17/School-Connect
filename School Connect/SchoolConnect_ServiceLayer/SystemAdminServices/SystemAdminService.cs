using SchoolConnect_DomainLayer.Models;
using SchoolConnect_RepositoryLayer.Interfaces;
using SchoolConnect_ServiceLayer.ISystemAdminServices;
using System.ComponentModel.DataAnnotations;

namespace SchoolConnect_ServiceLayer.SystemAdminServices
{
    public class SystemAdminService : ISystemAdminService
    {
        private readonly ISysAdmin _sysAdminRepo;

        public SystemAdminService(ISysAdmin sysAdminRepo)
        {
            _sysAdminRepo = sysAdminRepo;
        }

        public async Task<Dictionary<string, object>> CreateAdmin(SysAdmin admin)
        {
            try
            {
                if (admin == null)
                    throw new NullReferenceException("The system admin object passed into the CreateAdmin() method is null. Please review the System Admin Service in the ServiceLayer.");

                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(admin, serviceProvider: null, items: null);
                bool isValid = Validator.TryValidateObject(admin, validationContext, validationResults, true);

                List<string>? errors = [];
                if (!isValid)
                {
                    foreach (var validationResult in validationResults)
                        errors.Add(validationResult.ErrorMessage ?? "Something went wrong: Check the System Admin Service in the ServiceLayer.");

                    return new Dictionary<string, object>
                    {
                        { "Success", false },
                        { "Errors", errors }
                    };
                }

                Dictionary<string, object> result = await _sysAdminRepo.CreateAdmin(admin);
                return result;
            }
            catch (Exception ex)
            {
                return new Dictionary<string, object>
                {
                    { "Success", false },
                    { "ErrorMessage", ex.Message }
                };
            }
        }

        public async Task<Dictionary<string, object>> GetAdminById(long id)
        {
            try
            {
                Dictionary<string, object> result = await _sysAdminRepo.GetAdminById(id);
                return result;
            }
            catch (Exception ex)
            {
                return new Dictionary<string, object>
                {
                    { "Success", false },
                    { "ErrorMessage", ex.Message }
                };
            }
        }
        
        public async Task<Dictionary<string, object>> GetAdminByStaffNr(long staffNr)
        {
            try
            {
                if (staffNr.ToString().Length < 5 || staffNr.ToString().Length >= 8)
                {
                    return new Dictionary<string, object>
                    {
                        { "Success", false },
                        { "ErrorMessage", $"Invalid Staff Number. Staff should be between 5 and 8 digits in length. {staffNr} - {staffNr.ToString()} - {staffNr.ToString().Length} - {staffNr.ToString().Length >= 5 && staffNr.ToString().Length <= 8} - {staffNr.ToString().Length >= 5} - {staffNr.ToString().Length <= 8}" }
                    };
                }

                var result = await _sysAdminRepo.GetAdminByStaffNr(staffNr);
                return result;
            }
            catch (Exception ex)
            {
                return new Dictionary<string, object>
                {
                    { "Success", false },
                    { "ErrorMessage", ex.Message }
                };
            }
        }

        public async Task<Dictionary<string, object>> UpdateSystemAdmin(SysAdmin admin)
        {
            try
            {
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(admin, serviceProvider: null, items: null);
                bool isValid = Validator.TryValidateObject(admin, validationContext, validationResults);
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

                var result = await _sysAdminRepo.Update(admin);
                return result;
            }
            catch (Exception ex)
            {
                return new Dictionary<string, object>
                {
                    { "Success", false },
                    { "ErrorMeesage", ex.Message },
                };
            }
        }
    }
}
