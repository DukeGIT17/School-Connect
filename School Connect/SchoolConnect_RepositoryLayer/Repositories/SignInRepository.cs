using Microsoft.AspNetCore.Identity;
using SchoolConnect_RepositoryLayer.Interfaces;

namespace SchoolConnect_RepositoryLayer.Repositories
{
    public class SignInRepository : ISignIn
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private Dictionary<string, object> _returnDictionary;

        public SignInRepository(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
            _returnDictionary = [];
        }

        public async Task<Dictionary<string, object>> SignInAsync(string email, string password)
        {
            _returnDictionary = [];
            try
            {
                var result = await _signInManager.PasswordSignInAsync(email, password, false, false);

                if (!result.Succeeded)
                {
                    _returnDictionary.Add("Success", false);
                    _returnDictionary.Add("ErrorMessage", "Failed to sign user '{email}'.");
                    return _returnDictionary;
                }

                _returnDictionary.Add("Success", true);
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary.Add("Success", false);
                _returnDictionary.Add("ErrorMessage", ex.Message);
                return _returnDictionary;
            }
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
