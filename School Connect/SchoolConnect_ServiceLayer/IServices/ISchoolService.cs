using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_ServiceLayer.IServices
{
    public interface ISchoolService
    {
        Task<Dictionary<string, object>> RegisterSchool(School school);
        Task<Dictionary<string, object>> GetSchools();
    }
}
