namespace SchoolConnect_DomainLayer.Models
{
    public interface ITeacher
    {
        Task<Dictionary<string, object>> Create(Teacher teacher);
        Task<Dictionary<string, object>> Update(Teacher teacher);
        Task<Dictionary<string, object>> Get(long teacherId);
        Task<Dictionary<string, object>> GetTeacherByClass(string classId);
        Task<Dictionary<string, object>> GetAll();
        Task<Dictionary<string, object>> Remove(long teacherId);
    }
}
