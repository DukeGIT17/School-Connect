using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_RepositoryLayer.Interfaces
{
    public interface IAnnouncement
    {

        Task<Dictionary<string, object>> GetAnnByIdAsync(int announcementId);
        Task<Dictionary<string, object>> GetAllAnnBySchoolAsync(long schoolId);
        Task<Dictionary<string, object>> GetAnnouncementsByTeacherIdAsync(long teacherId);
        Task<Dictionary<string, object>> GetAnnouncementsByPrincipalIdAsync(long principalId);
        Task<Dictionary<string, object>> GetAnnouncementsById(int announcementId);
        Task<Dictionary<string, object>> CreateAsync(Announcement announcement);
        Task<Dictionary<string, object>> RemoveAsync(int announcementId);
        Task<Dictionary<string, object>> UpdateAnnouncementAsync(Announcement announcement);
    }
}
