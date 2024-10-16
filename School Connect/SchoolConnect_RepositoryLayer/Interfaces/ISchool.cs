using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_RepositoryLayer.Interfaces
{
    public interface ISchool
    {
        Task<Dictionary<string, object>> RegisterSchoolAsync(School school);
        Task<Dictionary<string, object>> GetSchoolsAsync();
        Task<Dictionary<string, object>> GetSchoolByName(string schoolName);
        Task<Dictionary<string, object>> GetSchoolByAdminAsync(long id);
        Task<Dictionary<string, object>> GetSchoolByIdAsync(long schoolId);
        Task<Dictionary<string, object>> GetSchoolByLearnerIdNoAsync(string learnerIdNo);
        Task<Dictionary<string, object>> UpdateSchoolInfo(School school);
        Task<Dictionary<string, object>> RemoveSchool(string emisNumber, long schoolId = -1);
    }
}
