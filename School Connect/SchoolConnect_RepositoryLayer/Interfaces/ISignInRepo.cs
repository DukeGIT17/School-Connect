namespace SchoolConnect_RepositoryLayer.Interfaces
{
    public interface ISignInRepo
    {

        Task<Dictionary<string, object>> SignInAsync(string email, string password);
        Task SignOutAsync();
    }
}
