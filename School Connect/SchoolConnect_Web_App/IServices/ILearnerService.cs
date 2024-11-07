using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_Web_App.IServices
{
    public interface ILearnerService
    {
        Task<Dictionary<string, object>> RegisterLearnerAsync(Learner learner);
        Task<Dictionary<string, object>> GetLearnerByIdNoAsync(string idNo);
        Task<Dictionary<string, object>> BulkLoadLearnersAsync(IFormFile file, long schoolId);
        Task<Dictionary<string, object>> GetLearnersByClassAsync(long teacherId);
        Task<Dictionary<string, object>> GetLearnersByClassIdAsync(int classId);
    }
}
