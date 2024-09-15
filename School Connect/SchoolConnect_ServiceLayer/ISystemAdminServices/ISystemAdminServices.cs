using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_ServiceLayer.ISystemAdminServices
{
    public interface ISystemAdminService
    {
        Task<Dictionary<string, object>> CreateAdmin(SysAdmin admin);
        Task<Dictionary<string, object>> GetAdminById(long id);
        Task<Dictionary<string, object>> GetAdminByStaffNr(long staffNr);
        Task<Dictionary<string, object>> UpdateSystemAdmin(SysAdmin admin);
    }
}
