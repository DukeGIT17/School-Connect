using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_Web_App.IServices
{
    public interface IAnnouncementService
    {
        Task<Dictionary<string, object>> CreateAnnouncementAsync(Announcement announcement);
        Task<Dictionary<string, object>> GetAnnouncementByPrincipalIdAsync(long principalId);
    }
}
