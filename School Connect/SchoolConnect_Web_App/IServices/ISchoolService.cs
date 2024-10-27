using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_Web_App.IServices
{
    public interface ISchoolService
    {
        Task<Dictionary<string, object>> RegisterSchoolAsync(School school);
        Task<Dictionary<string, object>> UpdateSchoolAsync(School school);
        Task<Dictionary<string, object>> GetSchools();
        Task<Dictionary<string, object>> GetSchoolByAdminAsync(long adminId);
        Task<Dictionary<string, object>> GetSchoolByLearnerIdNoAsync(string learnerIdNo);
        Task<Dictionary<string, object>> GetSchoolByIdAsync(long schoolId);
        Task<Dictionary<string, object>> GetAllClassesBySchoolAsync(long schoolId);
        Task<Dictionary<string, object>> GetClassBySchoolAsync(string classDesignate, long schoolId);
    }
}
