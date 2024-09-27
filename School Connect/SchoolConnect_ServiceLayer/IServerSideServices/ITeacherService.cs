using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_ServiceLayer.IServerSideServices
{
    public interface ITeacherService
    {
        Task<Dictionary<string, object>> CreateTeacherAsync(Teacher teacher);
    }
}
