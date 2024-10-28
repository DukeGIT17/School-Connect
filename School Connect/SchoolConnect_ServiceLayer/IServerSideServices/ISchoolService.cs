using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_ServiceLayer.IServerSideServices
{
    public interface ISchoolService
    {
        Task<Dictionary<string, object>> RegisterSchoolAsync(School school);
        Task<Dictionary<string, object>> GetSchoolsAsync();
        Task<Dictionary<string, object>> GetSchoolByIdAsync(long id);
        Task<Dictionary<string, object>> GetSchoolByLearnerIdNoAsync(string learnerIdNo);
        Task<Dictionary<string, object>> GetSchoolByAdminAsync(long adminId);
        Task<Dictionary<string, object>> UpdateSchoolInfoAsync(School school);
        Task<Dictionary<string, object>> DeleteSchoolAsync(School school);
        Task<Dictionary<string, object>> GetSchoolGradesAsync(long schoolid, string? fromGrade = null, string? toGrade = null);
        Task<Dictionary<string, object>> GetAllClassesBySchoolAsync(long schoolId);
        Task<Dictionary<string, object>> GetClassBySchoolAsync(string classDesignate, long schoolId);
        Task<Dictionary<string, object>> AddClassesToSchoolAsync(List<string> classDesignates, long schoolId);
    }
}
