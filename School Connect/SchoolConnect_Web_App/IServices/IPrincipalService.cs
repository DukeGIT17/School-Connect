using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_Web_App.IServices
{
    public interface IPrincipalService
    {
        Task<Dictionary<string, object>> RegisterPrincipalAsync(Principal principal);
        Task<Dictionary<string, object>> GetPrincipalByIdAsync(long principalId);
        Task<Dictionary<string, object>> UpdatePrincipalAsync(Principal principal);
    }
}
