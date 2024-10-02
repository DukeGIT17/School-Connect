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
                _returnDictionary = SharedValidationService.AttemptObjectValidation(principal);
                if (!(bool)_returnDictionary["Success"]) return _returnDictionary;

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

        public async Task<Dictionary<string, object>> GetById(long id)
        {
            try
            {
                return id < 1 ?
                    throw new($"The provided id '{id}' is less than one. Please provide a valid id.") :
                    await _principalRepo.GetById(id);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = true;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
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
