using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_RepositoryLayer.Interfaces
{
    public interface ISysAdmin
    {
        Task<Dictionary<string, object>> GetAdminByIdAsync(long sysAdminId);
        Task<Dictionary<string, object>> GetAdminByStaffNr(long staffNr);
        Task<Dictionary<string, object>> Update(SysAdmin sysAdmin);
    }
}
