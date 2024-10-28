using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_ServiceLayer.IServerSideServices
{
    public interface IAnnouncementService
    {
        Task<Dictionary<string, object>> CreateAnnouncementAsync(Announcement announcement);
        Task<Dictionary<string, object>> UpdateAsync(Announcement announcement);
        Task<Dictionary<string, object>> GetAnnouncementByIdAsync(int announcement);
        Task<Dictionary<string, object>> GetAnnouncementByPrincipalIdAsync(long principalId);
        Task<Dictionary<string, object>> GetAnnouncementsByTeacherIdAsync(long teacherId);
        Task<Dictionary<string, object>> GetAllAnnBySchool(long schoolId);
        Task<Dictionary<string, object>> RemoveAsync(int announcementId);
    }
}
