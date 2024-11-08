using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_Web_App.IServices
{
    public interface IParentService
    {
        Task<Dictionary<string, object>> RegisterParentAsync(Parent parent);
        Task<Dictionary<string, object>> BulkLoadParentsAsync(IFormFile file);
        Task<Dictionary<string, object>> GetParentByIdAsync(long id);
        Task<Dictionary<string, object>> UpdateParentInfo(Parent parent);
    }
}
