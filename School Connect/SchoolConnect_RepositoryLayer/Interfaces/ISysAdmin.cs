using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_RepositoryLayer.Interfaces
{
    public interface ISysAdmin
    {
        Task<Dictionary<string, object>> GetAdminByIdAsync(long sysAdminId);
        Task<Dictionary<string, object>> GetAdminByStaffNrAsync(string staffNr);
        Task<Dictionary<string, object>> UpdateAsync(SysAdmin sysAdmin);
    }
}
