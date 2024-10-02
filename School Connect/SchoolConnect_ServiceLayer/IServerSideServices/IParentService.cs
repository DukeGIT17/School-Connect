using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_ServiceLayer.IServerSideServices
{
    public interface IParentService
    {
        Task<Dictionary<string, object>> CreateAsync(Parent parent);
    }
}
