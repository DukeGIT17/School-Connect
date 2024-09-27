using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_ServiceLayer.IServerSideServices
{
    public interface ILearnerService
    {
        Task<Dictionary<string, object>> CreateAsync(Learner learner);
    }
}
