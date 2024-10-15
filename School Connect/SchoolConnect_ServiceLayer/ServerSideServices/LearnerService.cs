using SchoolConnect_DomainLayer.Models;
using SchoolConnect_RepositoryLayer.CommonAction;
using SchoolConnect_RepositoryLayer.Interfaces;
using SchoolConnect_ServiceLayer.IServerSideServices;

namespace SchoolConnect_ServiceLayer.ServerSideServices
{
    public class LearnerService : ILearnerService
    {
        private readonly ILearner _learnerRepo;
        private Dictionary<string, object> _returnDictionary;

        public LearnerService(ILearner learnerRepo)
        {
            _learnerRepo = learnerRepo;
            _returnDictionary = [];
        }

        public async Task<Dictionary<string, object>> LoadLearners(string fileName)
        {
            try
            {
                return await _learnerRepo.BatchLoadLearnersFromExcel(fileName);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> CreateAsync(Learner learner)
        {
            try
            {
                _returnDictionary = CommonActions.AttemptObjectValidation(learner);
                if (!(bool)_returnDictionary["Success"])
                    return _returnDictionary;

                foreach (var parent in learner.Parents)
                {
                    if (parent.Parent != null)
                    {
                        _returnDictionary = CommonActions.AttemptObjectValidation(parent.Parent);
                        if (!(bool)_returnDictionary["Success"])
                            return _returnDictionary;
                    }
                }

                _returnDictionary = await _learnerRepo.CreateAsync(learner);
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
                    await _learnerRepo.GetById(id);
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
    }
}
