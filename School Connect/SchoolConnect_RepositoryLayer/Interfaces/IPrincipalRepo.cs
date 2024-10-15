using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_RepositoryLayer.Interfaces
{
    public interface IPrincipalRepo
    {
        Task<Dictionary<string, object>> Update(Principal principal);
        Task<Dictionary<string, object>> GetById(long principalId);
        Task<Dictionary<string, object>> GetByStaffNr(string principalStaffNr);
        Task<Dictionary<string, object>> Create(Principal principal);
        Task<Dictionary<string, object>> Remove(long principalId);
    }
}
