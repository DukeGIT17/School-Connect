namespace SchoolConnect_ServiceLayer.IServices
{
    public interface ISignInService
    {
        Task<Dictionary<string, object>> SignInWithEmailAndPasswordAsync(string email, string password);
        Task SignOutAsync();
    }
}
