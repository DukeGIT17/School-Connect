using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_Web_App.IServices
{
    public interface ITeacherService
    {
        Task<Dictionary<string, object>> BulkLoadTeachersAsync(IFormFile teachersFile, long schoolId);
        Task<Dictionary<string, object>> RegisterTeacherAsync(Teacher teacher);
        Task<Dictionary<string, object>> GetTeacherByIdAsync(long teacherId);
        Task<Dictionary<string, object>> GetTeachersBySchoolAsync(long schoolId);
        Task<Dictionary<string, object>> GetTeacherByEmailAddressAsync(string email);
        Task<Dictionary<string, object>> UpdateTeacherAsync(Teacher teacher);
        Task<Dictionary<string, object>> UpdateClassAllocationAsync(Teacher teacher);
        Task<Dictionary<string, object>> GetAttendanceRecordsByTeacher(long teacherId);
        Task<Dictionary<string, object>> MarkAttendanceAsync(IEnumerable<Attendance> attendanceRecords);
        Task<Dictionary<string, object>> GetParentsByTeacherClassesAsync(long teacherId);
    }
}
