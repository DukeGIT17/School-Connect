using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_Web_App.IServices
{
    public interface ITeacherService
    {
        Task<Dictionary<string, object>> RegisterTeacherAsync(Teacher teacher);
    }
}
