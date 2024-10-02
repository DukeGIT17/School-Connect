using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_ServiceLayer.IServerSideServices
{
    public interface IParentService
    {
        Task<Dictionary<string, object>> CreateAsync(Parent parent);
        Task<Dictionary<string, object>> GetById(long id);
        Task<Dictionary<string, object>> GetByIdNo(long idNo);
    }
}
