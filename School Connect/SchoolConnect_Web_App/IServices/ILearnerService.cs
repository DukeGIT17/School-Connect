using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_Web_App.IServices
{
    public interface ILearnerService
    {
        Task<Dictionary<string, object>> RegisterLearnerAsync(Learner learner);
        Task<Dictionary<string, object>> GetLearnerByIdNo(string idNo);
        Task<Dictionary<string, object>> BulkLoadLearners(IFormFile file, long schoolId);
    }
}
