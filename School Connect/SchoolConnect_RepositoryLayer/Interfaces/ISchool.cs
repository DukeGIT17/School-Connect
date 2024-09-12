using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_RepositoryLayer.Interfaces
{
    public interface ISchool
    {
        Task<Dictionary<string, object>> RegisterSchool(School school);
        Task<Dictionary<string, object>> GetSchools();
        Task<Dictionary<string, object>> GetSchoolsByName(string schoolName);
        Task<Dictionary<string, object>> GetSchoolsById(long schoolId);
        Task<Dictionary<string, object>> GetSchoolsByChild(long childId);
        Task<Dictionary<string, object>> UpdateSchoolInfo(School school);
        Task<Dictionary<string, object>> RemoveSchool(long emisNumber, long schoolId = -1);
    }
}
