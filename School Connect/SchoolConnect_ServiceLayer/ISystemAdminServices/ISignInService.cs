using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_ServiceLayer.ISystemAdminServices
{
    public interface ISignInService
    {
        Task<Dictionary<string, object>> SignInAsync(LoginModel model);
        void SignOutAsync();
    }
}
