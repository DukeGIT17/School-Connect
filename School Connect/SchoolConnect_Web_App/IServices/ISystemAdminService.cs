using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_Web_App.IServices
{
    public interface ISystemAdminService
    {
        Task<Dictionary<string, object>> GetAdminByIdAsync(long systemAdminId);
        Task<Dictionary<string, object>> GetAdminByStaffNr(string staffNr);
        Task<Dictionary<string, object>> UpdateAsync(SysAdmin systemAdmin);

    }
}
