using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_ServiceLayer.IServerSideServices
{
    public interface ISchoolService
    {
        Task<Dictionary<string, object>> RegisterSchoolAsync(School school);
        Task<Dictionary<string, object>> GetSchoolsAsync();
        Task<Dictionary<string, object>> GetSchoolByIdAsync(long id);
        Task<Dictionary<string, object>> GetSchoolByAdminAsync(long adminId);
        Task<Dictionary<string, object>> UpdateSchoolInfoAsync(School school);
        Task<Dictionary<string, object>> DeleteSchoolAsync(School school);
    }
}
