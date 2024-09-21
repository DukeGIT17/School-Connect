using SchoolConnect_DomainLayer.Models;
using SchoolConnect_RepositoryLayer.Interfaces;
using SchoolConnect_ServiceLayer.IServerSideServices;
using System.ComponentModel.DataAnnotations;

namespace SchoolConnect_ServiceLayer.ServerSideServices
{
    public class AdminService : ISystemAdminService
    {
        private readonly ISysAdmin _sysAdminRepo;
        private Dictionary<string, object> _returnDictionary;

        public AdminService(ISysAdmin sysAdminRepo)
        {
            _sysAdminRepo = sysAdminRepo;
            _returnDictionary = [];
        }

        public async Task<Dictionary<string, object>> GetAdminById(long id)
        {
            try
            {
                return _returnDictionary = id < 1 
                    ? throw new Exception($"Invalid ID. Provided ID '{id}' was less than 1 (One).") 
                    : await _sysAdminRepo.GetAdminById(id);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }
        
        public async Task<Dictionary<string, object>> GetAdminByStaffNr(long staffNr)
        {
            try
            {
                if (staffNr.ToString().Length < 5 || staffNr.ToString().Length >= 8)
                    throw new Exception($"Invalid Staff Number. Staff should be between 5 and 8 digits in length.");

                _returnDictionary = await _sysAdminRepo.GetAdminByStaffNr(staffNr);
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
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

                    _returnDictionary["Success"] = false;
                    _returnDictionary["Errors"] = errors;
                    return _returnDictionary;
                }

                _returnDictionary = await _sysAdminRepo.Update(admin);
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }
    }
}
