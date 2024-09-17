using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_RepositoryLayer.Interfaces
{
    public interface ISignInRepo
    {

        Task<Dictionary<string, object>> SignInAsync(LoginModel loginModel);
        Task<Dictionary<string, object>> SetNewPasswordAsync(LoginModel loginModel);
        void SignOutAsync();
    }
}
