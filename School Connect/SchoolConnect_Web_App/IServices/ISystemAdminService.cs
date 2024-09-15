using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_Web_App.IServices
{
    public interface ISystemAdminService
    {
        Dictionary<string, object> GetAdminById(long systemAdminId);
        Dictionary<string, object> GetAdminByStaffNr(long staffNr);
        Dictionary<string, object> Update(SysAdmin systemAdmin);

    }
}
