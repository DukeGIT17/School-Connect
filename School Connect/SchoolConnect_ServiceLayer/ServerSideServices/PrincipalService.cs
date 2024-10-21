using SchoolConnect_DomainLayer.Models;
using static SchoolConnect_RepositoryLayer.CommonAction.CommonActions;
using SchoolConnect_RepositoryLayer.Interfaces;
using SchoolConnect_ServiceLayer.IServerSideServices;

namespace SchoolConnect_ServiceLayer.ServerSideServices
{
    public class PrincipalService : IPrincipalService
    {
        private readonly IPrincipalRepo _principalRepo;
        private Dictionary<string, object> _returnDictionary;

        public PrincipalService(IPrincipalRepo principalService)
        {
            _principalRepo = principalService;
            _returnDictionary = [];
        }

        public async Task<Dictionary<string, object>> Create(Principal principal)
        {
            try
            {
                _returnDictionary = AttemptObjectValidation(principal);
                if (!(bool)_returnDictionary["Success"]) return _returnDictionary;

                _returnDictionary = await _principalRepo.CreateAsync(principal);
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
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public Task<Dictionary<string, object>> GetByStaffNr(string staffNr)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, object>> Remove(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<Dictionary<string, object>> UpdateAsync(Principal principal)
        {
            try
            {
                _returnDictionary = AttemptObjectValidation(principal);
                if (!(bool)_returnDictionary["Success"]) return _returnDictionary;

                _returnDictionary = await _principalRepo.UpdateAsync(principal);
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = true;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }
    }
}
