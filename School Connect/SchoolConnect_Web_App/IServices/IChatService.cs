using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_Web_App.IServices
{
    public interface IChatService
    {
        Task<Dictionary<string, object>> SaveMessageAsync(Chat chatMessage);
    }
}
