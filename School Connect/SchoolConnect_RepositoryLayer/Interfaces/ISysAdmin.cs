using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_RepositoryLayer.Interfaces
{
    public interface ISysAdmin
    {
        Task<Dictionary<string, object>> CreateAdmin(SysAdmin systemAdmin);
        Task<Dictionary<string, object>> GetAdminById(long sysAdminId);
        Task<Dictionary<string, object>> GetAdminByStaffNr(long staffNr);
        Task<Dictionary<string, object>> Update(SysAdmin sysAdmin);
    }
}
