using Microsoft.AspNetCore.Http;
using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_ServiceLayer.IServerSideServices
{
    public interface ILearnerService
    {
        Task<Dictionary<string, object>> LoadLearnersAsync(IFormFile file, long schoolId);
        Task<Dictionary<string, object>> CreateAsync(Learner learner);
        Task<Dictionary<string, object>> UpdateAsync(Learner learner);
        Task<Dictionary<string, object>> GetLearnerByIdAsync(long id);
        Task<Dictionary<string, object>> GetLearnerByIdNoAsync(string idNo);
        Task<Dictionary<string, object>> GetLearnersByMainTeacherAsync(long teacherId);
    }
}
