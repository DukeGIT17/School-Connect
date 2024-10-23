using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_Web_App.IServices
{
    public interface IAnnouncementService
    {
        Task<Dictionary<string, object>> CreateAnnouncementAsync(Announcement announcement);
        Task<Dictionary<string, object>> GetAnnouncementByPrincipalIdAsync(long principalId);
        Task<Dictionary<string, object>> GetAllAnnBySchoolAsync(long schoolId);
        Task<Dictionary<string, object>> RemoveAnnouncementAsync(int announcementId);
        Task<Dictionary<string, object>> GetAnnouncementById(int announcementId);
        Task<Dictionary<string, object>> GetAnnouncementByTeacherId(long teacherId);
    }
}
