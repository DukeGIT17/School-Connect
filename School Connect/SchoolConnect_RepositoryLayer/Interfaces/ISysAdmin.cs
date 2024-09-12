using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_RepositoryLayer.Interfaces
{
    public interface ISysAdmin
    {
        Task<Dictionary<string, object>> Get(long sysAdminId);
        Task<Dictionary<string, object>> Update(SysAdmin sysAdmin);
    }
}
