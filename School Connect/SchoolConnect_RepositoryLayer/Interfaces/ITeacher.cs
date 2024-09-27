using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_RepositoryLayer.Interfaces
{
    public interface ITeacher
    {
        Task<Dictionary<string, object>> CreateAsync(Teacher teacher);
        Task<Dictionary<string, object>> Update(Teacher teacher);
        Task<Dictionary<string, object>> Get(long teacherId);
        Task<Dictionary<string, object>> GetTeacherByClass(string classId);
        Task<Dictionary<string, object>> GetAll();
        Task<Dictionary<string, object>> Remove(long teacherId);
    }
}
