using Microsoft.AspNetCore.Http;
using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_ServiceLayer.IServerSideServices
{
    public interface ILearnerService
    {
        Task<Dictionary<string, object>> LoadLearners(IFormFile file, long schoolId);
        Task<Dictionary<string, object>> CreateAsync(Learner learner);
        Task<Dictionary<string, object>> UpdateAsync(Learner learner);
        Task<Dictionary<string, object>> GetById(long id);
        Task<Dictionary<string, object>> GetByIdNo(string idNo);
    }
}
