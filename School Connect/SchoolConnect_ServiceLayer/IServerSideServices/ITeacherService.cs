using Microsoft.AspNetCore.Http;
using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_ServiceLayer.IServerSideServices
{
    public interface ITeacherService
    {
        Task<Dictionary<string, object>> BulkLoadTeacherAsync(IFormFile teacherFile, long schoolId);
        Task<Dictionary<string, object>> CreateTeacherAsync(Teacher teacher);
        Task<Dictionary<string, object>> GetById(long id);
        Task<Dictionary<string, object>> GetByStaffNr(string staffNr);
    }
}
