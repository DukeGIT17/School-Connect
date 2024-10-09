using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_ServiceLayer.IServerSideServices
{
    public interface IAnnouncementService
    {
        Task<Dictionary<string, object>> CreateAnnouncementAsync(Announcement announcement);
        Task<Dictionary<string, object>> GetAnnouncementByIdAsync(int announcement);
    }
}
