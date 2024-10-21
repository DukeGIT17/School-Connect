using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_ServiceLayer.IServerSideServices
{
    public interface IPrincipalService
    {
        Task<Dictionary<string, object>> Create(Principal principal);
        Task<Dictionary<string, object>> UpdateAsync(Principal principal);
        Task<Dictionary<string, object>> GetById(long id);
        Task<Dictionary<string, object>> GetByStaffNr(string staffNr);
        Task<Dictionary<string, object>> Remove(long id);
    }
}
