namespace SchoolConnect_Web_App.IServices
{
    public interface ISignInService
    {
        Task<Dictionary<string, object>> SignInWithEmailAndPasswordAsync(string email, string password);
        Task<Dictionary<string, object>> SignOutAsync();
    }
}
