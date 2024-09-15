namespace SchoolConnect_Web_App.IServices
{
    public interface ISignInService
    {
        Dictionary<string, object> SignInWithEmailAndPasswordAsync(string email, string password);
        void SignOutAsync();
    }
}
