using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_ServiceLayer.ISchoolServices
{
    public interface ISchoolService
    {
        Task<Dictionary<string, object>> RegisterSchoolAsync(School school);
        Task<Dictionary<string, object>> GetSchoolsAsync();
        Task<Dictionary<string, object>> GetSchoolById(long id);
        Task<Dictionary<string, object>> GetSchoolByEmisNumber(long emisNumber);
        Task<Dictionary<string, object>> UpdateSchoolInfoAsync(School school);
        Task<Dictionary<string, object>> DeleteSchoolAsync(School school);
    }
}
