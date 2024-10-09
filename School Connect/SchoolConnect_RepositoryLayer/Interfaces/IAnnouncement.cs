using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_RepositoryLayer.Interfaces
{
    public interface IAnnouncement
    {

        Task<Dictionary<string, object>> GetAnnByIdAsync(int announcementId);
        Task<Dictionary<string, object>> GetAllRelevantAnnouncements();
        Task<Dictionary<string, object>> GetAnnouncementsByTeacher(long teacherId);
        Task<Dictionary<string, object>> GetAnnouncementsById(int announcementId);
        Task<Dictionary<string, object>> CreateAsync(Announcement announcement);
        Task<Dictionary<string, object>> Remove(int announcementId);
        Task<Dictionary<string, object>> Update(Announcement announcement);
    }
}
