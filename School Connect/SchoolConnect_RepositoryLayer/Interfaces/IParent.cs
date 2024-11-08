using Microsoft.AspNetCore.Http;
using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_RepositoryLayer.Interfaces
{
    public interface IParent
    {
        Task<Dictionary<string, object>> BatchLoadParentsFromExcelAsync(IFormFile file);
        Task<Dictionary<string, object>> CreateAsync(Parent parent);
        Task<Dictionary<string, object>> UpdateAsync(Parent parent);
        Task<Dictionary<string, object>> GetParentByIdAsync(long parentId);
        Task<Dictionary<string, object>> GetByIdNoAsync(string parentIdNo);
        Task<Dictionary<string, object>> GetTeachersByParentAsync(long parentId);
    }
}
