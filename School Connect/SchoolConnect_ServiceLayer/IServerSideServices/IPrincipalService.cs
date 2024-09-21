using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_ServiceLayer.IServerSideServices
{
    public interface IPrincipalService
    {
        Task<Dictionary<string, object>> Create(Principal principal);
        Task<Dictionary<string, object>> Update(Principal principal);
        Task<Dictionary<string, object>> GetById(long id);
        Task<Dictionary<string, object>> GetByStaffNr(long staffNr);
        Task<Dictionary<string, object>> Remote(long id, long? staffNr = -1);
    }
}
