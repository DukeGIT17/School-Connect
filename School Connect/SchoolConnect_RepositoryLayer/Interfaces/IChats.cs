using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_RepositoryLayer.Interfaces
{
    public interface IChats
    {
        Task<Dictionary<string, object>> SaveMessageAsync(Chat chatMessage);
    }
}
