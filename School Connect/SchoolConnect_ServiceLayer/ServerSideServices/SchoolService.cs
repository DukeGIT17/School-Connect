using SchoolConnect_DomainLayer.Models;
using SchoolConnect_RepositoryLayer.Interfaces;
using SchoolConnect_ServiceLayer.IServerSideServices;
using static SchoolConnect_RepositoryLayer.CommonAction.CommonActions;

namespace SchoolConnect_ServiceLayer.ServerSideServices
{
    public class SchoolService(ISchool schoolRepo) : ISchoolService
    {
        private readonly ISchool _schoolRepo = schoolRepo;
        private Dictionary<string, object> _returnDictionary = [];

        public async Task<Dictionary<string, object>> AddClassesToSchoolAsync(List<string> classDesignates, long schoolId)
        {
            try
            {
                return await _schoolRepo.AddClassesToSchool(classDesignates, schoolId);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> DeleteSchoolAsync(School school)
        {
            throw new NotImplementedException();
        }

        public async Task<Dictionary<string, object>> GetAllClassesBySchoolAsync(long schoolId)
        {
            try
            {
                return await _schoolRepo.GetAllClassesBySchoolAsync(schoolId);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetClassBySchoolAsync(string classDesignate, long schoolId)
        {
            try
            {
                return await _schoolRepo.GetClassBySchoolAsync(classDesignate, schoolId);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
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

        public async Task<Dictionary<string, object>> GetSchoolGradesAsync(long schoolid, string? fromGrade = null, string? toGrade = null)
        {
            try
            {
                if (fromGrade is not null)
                {
                    bool converted = int.TryParse(fromGrade, out int value);
                    if (!converted && fromGrade != "R") throw new("Something went wrong, fromGrade could not be converted to int and is not 'R'. Please provide a valid grade.");
                    if ((value < 1 && fromGrade != "R") || value > 12) throw new("fromGrade value provided was less than one and not 'R' or was greater than 12");
                }

                if (toGrade is not null)
                {
                    bool converted = int.TryParse(toGrade, out int value);
                    if (!converted && toGrade != "R") throw new("Something went wrong, toGrade could not be converted to int and is not 'R'. Please provide a valid grade.");
                    if ((value < 1 && toGrade != "R") || value > 12) throw new("toGrade value provided was less than one and not 'R' or was greater than 12");

                    char? testChar = toGrade.Last();
                    if (testChar is null) throw new("Invalid class designation. Please provide a valid class.");
                }

                if (fromGrade is not null && toGrade is not null)
                {
                    if (Convert.ToInt32(fromGrade) > Convert.ToInt32(toGrade)) throw new("Something went wrong, fromGrade filter value cannot be greater than the toGrade value.");
                }

                return await _schoolRepo.GetSchoolGradesAsync(schoolid, fromGrade, toGrade);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetSchoolsAsync()
        {
            try 
            {
                return await _schoolRepo.GetSchoolsAsync();
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

                return  await _schoolRepo.RegisterSchoolAsync(school);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> UpdateSchoolInfoAsync(School school)
        {
            try
            {
                return await _schoolRepo.UpdateSchoolInfoAsync(school);
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
