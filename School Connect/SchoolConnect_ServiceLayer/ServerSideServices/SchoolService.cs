using SchoolConnect_DomainLayer.Models;
using SchoolConnect_RepositoryLayer.CommonAction;
using SchoolConnect_RepositoryLayer.Interfaces;
using SchoolConnect_ServiceLayer.IServerSideServices;
using static SchoolConnect_RepositoryLayer.CommonAction.CommonActions;

namespace SchoolConnect_ServiceLayer.ServerSideServices
{
    public class SchoolService(ISchool schoolRepo) : ISchoolService
    {
        private readonly ISchool _schoolRepo = schoolRepo;
        private Dictionary<string, object> _returnDictionary = [];

        public Task<Dictionary<string, object>> DeleteSchoolAsync(School school)
        {
            throw new NotImplementedException();
        }

        public async Task<Dictionary<string, object>> GetSchoolByAdminAsync(long adminId)
        {
            try
            {
                return await _schoolRepo.GetSchoolByAdminAsync(adminId);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetSchoolByIdAsync(long id)
        {
            try
            {
                return await _schoolRepo.GetSchoolByIdAsync(id);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message + "\nInner Exception: " + ex.InnerException;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetSchoolByLearnerIdNoAsync(string learnerIdNo)
        {
            try
            {
                return await _schoolRepo.GetSchoolByLearnerIdNoAsync(learnerIdNo);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message + "\nInner Exception: " + ex.InnerException;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetSchoolsAsync()
        {
            try 
            {
                _returnDictionary = await _schoolRepo.GetSchoolsAsync();
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> RegisterSchoolAsync(School school)
        {
            try
            {
                _returnDictionary = AttemptObjectValidation(school);
                if (!(bool)_returnDictionary["Success"]) return _returnDictionary;

                _returnDictionary = await _schoolRepo.RegisterSchoolAsync(school);
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public Task<Dictionary<string, object>> UpdateSchoolInfoAsync(School school)
        {
            throw new NotImplementedException();
        }
    }
}
