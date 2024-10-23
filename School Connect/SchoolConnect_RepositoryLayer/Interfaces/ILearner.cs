using Microsoft.AspNetCore.Http;
using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_RepositoryLayer.Interfaces
{
    public interface ILearner
    {
        Task<Dictionary<string, object>> BatchLoadLearnersFromExcel(IFormFile file, long schoolId);
        Task<Dictionary<string, object>> CreateAsync(Learner learner);
        Task<Dictionary<string, object>> GetById(long learnerId);
        Task<Dictionary<string, object>> GetByIdNo(string learnerIdNo);
        Task<Dictionary<string, object>> GetAll();
        Task<Dictionary<string, object>> GetClass(string classId);
        Task<Dictionary<string, object>> UpdateAsync(Learner learner);
        Task<Dictionary<string, object>> Remove(long learnerId);
        Task<Dictionary<string, object>> RemoveClass(string classId);
    }
}
