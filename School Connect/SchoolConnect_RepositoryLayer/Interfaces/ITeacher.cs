using Microsoft.AspNetCore.Http;
using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_RepositoryLayer.Interfaces
{
    public interface ITeacher
    {
        Task<Dictionary<string, object>> BulkLoadTeacherFromExcel(IFormFile file, long schoolId);
        Task<Dictionary<string, object>> CreateAsync(Teacher teacher);
        Task<Dictionary<string, object>> UpdatePersonalInfoAsync(Teacher teacher);
        Task<Dictionary<string, object>> UpdateClassAllocationAsync(Teacher teacher);
        Task<Dictionary<string, object>> GetByIdAsync(long teacherId);
        Task<Dictionary<string, object>> GetTeachersBySchoolAsync(long schoolId);
        Task<Dictionary<string, object>> GetTeacherByClass(string classId);
        Task<Dictionary<string, object>> GetTeacherByEmailAddressAsync(string email);
        Task<Dictionary<string, object>> GetAttendanceRecordsByTeacherAsync(long teacherId);
        Task<Dictionary<string, object>> MarkClassAttendanceAsync(IEnumerable<Attendance> attendanceRecords);
        Task<Dictionary<string, object>> Remove(long teacherId);
    }
}
