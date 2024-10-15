using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_ServiceLayer.IServerSideServices
{
    public interface ILearnerService
    {
        Task<Dictionary<string, object>> LoadLearners(string fileName);
        Task<Dictionary<string, object>> CreateAsync(Learner learner);
        Task<Dictionary<string, object>> GetById(long id);
        Task<Dictionary<string, object>> GetByIdNo(string idNo);
    }
}
