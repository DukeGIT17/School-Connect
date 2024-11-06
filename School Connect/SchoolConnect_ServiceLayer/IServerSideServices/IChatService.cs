using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_ServiceLayer.IServerSideServices
{
    public interface IChatService
    {
        Task<Dictionary<string, object>> SaveMessageAsync(Chat chatMessage);
    }
}
