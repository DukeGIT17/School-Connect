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

        public Task<Dictionary<string, object>> GetSchoolsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Dictionary<string, object>> RegisterSchoolAsync(School school)
        {
            try
            {
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(school, serviceProvider: null, items: null);
                bool isValid = Validator.TryValidateObject(school, validationContext, validationResults);
                List<string>? errors = [];

                if (!isValid)
                {
                    foreach (var validationResult in validationResults)
                        errors.Add(validationResult.ErrorMessage
                            ?? "Something went wrong: Check the School Service in the ServiceLayer.");

                    _returnDictionary["Success"] = false;
                    _returnDictionary["Errors"] = errors;
                    return _returnDictionary;
                }

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
