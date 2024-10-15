using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_ServiceLayer.IServerSideServices
{
    public interface ISignInService
    {
        Task<Dictionary<string, object>> SignInAsync(LoginModel model);
        Task<Dictionary<string, object>> SetNewPasswordAsync(LoginModel model);
        void SignOutAsync();
        void IsSignedIn();
    }
}
