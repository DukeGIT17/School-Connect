using Microsoft.Identity.Client;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_RepositoryLayer.Interfaces;
using SchoolConnect_ServiceLayer.IServerSideServices;
using System.ComponentModel.DataAnnotations;

namespace SchoolConnect_ServiceLayer.ServerSideServices
{
    public class PrincipalService : IPrincipalService
    {
        private readonly IPrincipal _principalRepo;
        private Dictionary<string, object> _returnDictionary;

        public PrincipalService(IPrincipal principalService)
        {
            _principalRepo = principalService;
            _returnDictionary = [];
        }

        public async Task<Dictionary<string, object>> Create(Principal principal)
        {
            try
            {
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(principal, serviceProvider: null, items: null);
                bool isValid = Validator.TryValidateObject(principal, validationContext, validationResults);
                List<string>? errors = [];

                if (!isValid)
                {
                    foreach(var validationResult in validationResults)
                        errors.Add(validationResult.ErrorMessage
                            ?? "Something went wrong: Check the School Service in the ServiceLayer.");

                    _returnDictionary["Success"] = false;
                    _returnDictionary["Errors"] = errors;
                    return _returnDictionary;
                }

                _returnDictionary = await _principalRepo.Create(principal);
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public Task<Dictionary<string, object>> GetById(long id)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, object>> GetByStaffNr(long staffNr)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, object>> Remote(long id, long? staffNr = -1)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, object>> Update(Principal principal)
        {
            throw new NotImplementedException();
        }
    }
}
