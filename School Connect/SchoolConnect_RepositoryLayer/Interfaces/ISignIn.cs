namespace SchoolConnect_RepositoryLayer.Interfaces
{
    public interface ISignIn
    {
        Task<Dictionary<string, object>> SignInAsync(string email, string password);
        Task SignOutAsync();
    }
}
