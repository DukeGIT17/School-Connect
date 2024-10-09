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
        Task<Dictionary<string, object>> GetSchoolByChild(long childId);
        Task<Dictionary<string, object>> UpdateSchoolInfo(School school);
        Task<Dictionary<string, object>> RemoveSchool(long emisNumber, long schoolId = -1);
    }
}
