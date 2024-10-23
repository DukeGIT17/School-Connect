using Microsoft.AspNetCore.Http;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_RepositoryLayer.CommonAction;
using SchoolConnect_RepositoryLayer.Interfaces;
using SchoolConnect_ServiceLayer.IServerSideServices;

namespace SchoolConnect_ServiceLayer.ServerSideServices
{
    public class ParentService(IParent parentRepo) : IParentService
    {
        private readonly IParent _parentRepo = parentRepo;
        private Dictionary<string, object> _returnDictionary = [];

        public async Task<Dictionary<string, object>> BatchLoadParentsAsync(IFormFile parentFile)
        {
            try
            {
                return await _parentRepo.BatchLoadParentsFromExcel(parentFile);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> CreateAsync(Parent parent)
        {
            try
            {
                _returnDictionary = CommonActions.AttemptObjectValidation(parent);
                if (!(bool)_returnDictionary["Success"])
                    return _returnDictionary;

                foreach (var learner in parent.Children!)
                {
                    if (learner.Learner != null)
                    {
                        _returnDictionary = CommonActions.AttemptObjectValidation(learner.Learner);
                        if (!(bool)_returnDictionary["Success"])
                            return _returnDictionary;
                    }
                }

                _returnDictionary = await _parentRepo.CreateAsync(parent);
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
                    await _parentRepo.GetById(id);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = true;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public Task<Dictionary<string, object>> GetByIdNo(string idNo)
        {
            throw new NotImplementedException();
        }

        public async Task<Dictionary<string, object>> UpdateAsync(Parent parent)
        {
            try
            {
                return await _parentRepo.UpdateAsync(parent);
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
