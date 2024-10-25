using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_RepositoryLayer.Interfaces
{
    public interface ISignInRepo
    { 
        Task<Dictionary<string, object>> SignInAsync(LoginModel loginModel);
        Task<Dictionary<string, object>> CreateUserAccountAsync(string email, string role, string? phoneNumber = null);
        Task<Dictionary<string, object>> RemoveUserAccountAsync(string email, string role);
        Task<Dictionary<string, object>> SetNewPasswordAsync(LoginModel loginModel);
        Task<Dictionary<string, object>> ChangeEmailAddressAsync(string oldEmail, string newEmail);
        Task<Dictionary<string, object>> ChangePhoneNumberAsync(string oldPhoneNumber, string newPhoneNumber, string email);
        void SignOutAsync();
        void IsSignedIn();
    }
}
