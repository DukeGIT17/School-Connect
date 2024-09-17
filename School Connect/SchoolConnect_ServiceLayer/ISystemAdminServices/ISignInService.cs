using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_ServiceLayer.ISystemAdminServices
{
    public interface ISignInService
    {
        Task<Dictionary<string, object>> SignInAsync(LoginModel model);
        Task<Dictionary<string, object>> SetNewPasswordAsync(LoginModel model);
        void SignOutAsync();
    }
}
