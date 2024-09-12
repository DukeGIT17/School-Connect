using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_RepositoryLayer.Interfaces
{
    public interface ILearner
    {
        Task<Dictionary<string, object>> Create(Learner learner);
        Task<Dictionary<string, object>> Get(long learnerId);
        Task<Dictionary<string, object>> GetAll();
        Task<Dictionary<string, object>> GetClass(string classId);
        Task<Dictionary<string, object>> UpdateLearner(Learner learner);
        Task<Dictionary<string, object>> Remove(long learnerId);
        Task<Dictionary<string, object>> RemoveClass(string classId);
    }
}
