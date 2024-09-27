using SchoolConnect_DomainLayer.Models;
using SchoolConnect_RepositoryLayer.Interfaces;
using SchoolConnect_ServiceLayer.IServerSideServices;
using System.ComponentModel.DataAnnotations;

namespace SchoolConnect_ServiceLayer.ServerSideServices
{
    public class SchoolService : ISchoolService
    {
        private readonly ISchool _schoolRepo;
        private Dictionary<string, object> _returnDictionary;

        public SchoolService(ISchool schoolRepo)
        {
            _schoolRepo = schoolRepo;
            _returnDictionary = [];
        }

        public Task<Dictionary<string, object>> DeleteSchoolAsync(School school)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, object>> GetSchoolByEmisNumber(long emisNumber)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, object>> GetSchoolById(long id)
        {
            throw new NotImplementedException();
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
                _returnDictionary = SharedValidationService.AttemptObjectValidation(school);
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
