using Microsoft.AspNetCore.Http;
using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_ServiceLayer.IServerSideServices
{
    public interface ITeacherService
    {
        Task<Dictionary<string, object>> BulkLoadTeacherAsync(IFormFile teacherFile, long schoolId);
        Task<Dictionary<string, object>> CreateTeacherAsync(Teacher teacher);
        Task<Dictionary<string, object>> GetByIdAsync(long id);
        Task<Dictionary<string, object>> GetTeachersBySchoolAsync(long schoolId);
        Task<Dictionary<string, object>> GetByStaffNr(string staffNr);
        Task<Dictionary<string, object>> GetTeacherByEmailAddressAsync(string email);
        Task<Dictionary<string, object>> UpdatePersonalInfoAsync(Teacher teacher);
        Task<Dictionary<string, object>> UpdateClassAllocationAsync(Teacher teacher);
        Task<Dictionary<string, object>> GetAttendanceRecordsByTeacherAsync(long teacherId);
        Task<Dictionary<string, object>> MarkClassAttendanceAsync(IEnumerable<Attendance> attendanceRecords);
    }
}
