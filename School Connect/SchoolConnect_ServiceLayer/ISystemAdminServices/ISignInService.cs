namespace SchoolConnect_ServiceLayer.ISystemAdminServices
{
    public interface ISignInService
    {
        Task<Dictionary<string, object>> SignInAsync(string email, string password);
        Task SignInAsync(string email, string password);
    }
}
