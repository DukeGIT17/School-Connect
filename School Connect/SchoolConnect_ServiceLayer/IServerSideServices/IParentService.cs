using Microsoft.AspNetCore.Http;
using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_ServiceLayer.IServerSideServices
{
    public interface IParentService
    {
        Task<Dictionary<string, object>> BatchLoadParentsAsync(IFormFile parentFile);
        Task<Dictionary<string, object>> CreateAsync(Parent parent);
        Task<Dictionary<string, object>> UpdateAsync(Parent parent);
        Task<Dictionary<string, object>> GetParentByIdAsync(long id);
        Task<Dictionary<string, object>> GetByIdNo(string idNo);
    }
}
