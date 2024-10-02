using SchoolConnect_DomainLayer.Models;
using SchoolConnect_RepositoryLayer.Interfaces;
using SchoolConnect_ServiceLayer.IServerSideServices;

namespace SchoolConnect_ServiceLayer.ServerSideServices
{
    public class ParentService(IParent parentRepo) : IParentService
    {
        private readonly IParent _parentRepo = parentRepo;
        private Dictionary<string, object> _returnDictionary = [];
        
        public async Task<Dictionary<string, object>> CreateAsync(Parent parent)
        {
            try
            {
                _returnDictionary = SharedValidationService.AttemptObjectValidation(parent);
                if (!(bool)_returnDictionary["Success"])
                    return _returnDictionary;

                foreach (var learner in parent.Children!)
                {
                    if (learner.Learner != null)
                    {
                        _returnDictionary = SharedValidationService.AttemptObjectValidation(learner.Learner);
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
    }
}
