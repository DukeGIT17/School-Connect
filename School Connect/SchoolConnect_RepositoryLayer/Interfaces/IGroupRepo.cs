using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_RepositoryLayer.Interfaces
{
    public interface IGroupRepo
    {
        Task<Dictionary<string, object>> AddToGroup(string actorId, long schoolId, string groupName);
        Dictionary<string, object> AddTeacherToGroup(ref Teacher teacher, long schoolId, string groupName);
        Dictionary<string, object> AddParentToGroup(Parent parent, long schoolId, string groupName);
    }
}
