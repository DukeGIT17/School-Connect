using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_ServiceLayer.IServerSideServices
{
    public interface ISystemAdminService
    {
        Task<Dictionary<string, object>> GetAdminById(long id);
        Task<Dictionary<string, object>> GetAdminByStaffNr(string staffNr);
        Task<Dictionary<string, object>> UpdateSystemAdmin(SysAdmin admin);
    }
}
