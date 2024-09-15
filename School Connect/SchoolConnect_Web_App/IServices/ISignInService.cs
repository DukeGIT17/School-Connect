using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_Web_App.IServices
{
    public interface ISignInService
    {
        Dictionary<string, object> SignInWithEmailAndPassword(LoginModel model);
        void SignOut();
    }
}
