using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_Web_App.IServices
{
    public interface ILearnerService
    {
        Task<Dictionary<string, object>> RegisterLearnerAsync(Learner learner);
    }
}
