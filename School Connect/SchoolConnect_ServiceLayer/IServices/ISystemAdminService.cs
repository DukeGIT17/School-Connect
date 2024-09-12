using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_ServiceLayer.IServices
{
    public interface ISystemAdminService
    {
        Task<Dictionary<string, object>> GetAdminById(long systemAdminId);
        Task<Dictionary<string, object>> GetAdminByStaffNr(long staffNr);
        Task<Dictionary<string, object>> Update(SysAdmin systemAdmin);

    }
}
