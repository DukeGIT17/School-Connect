using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_Web_App.IServices
{
    public interface IParentService
    {
        Task<Dictionary<string, object>> RegisterParentAsync(Parent parent);
    }
}
