using Microsoft.AspNetCore.Identity;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_RepositoryLayer.Interfaces;

namespace SchoolConnect_RepositoryLayer.Repositories
{
    public class SignInRepository : ISignInRepo
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private Dictionary<string, object> _returnDictionary;

        public SignInRepository(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
            _returnDictionary = [];
        }

        public async Task<Dictionary<string, object>> SignInAsync(LoginModel model)
        {
            _returnDictionary = [];
            try
            {
                var result = await _signInManager.PasswordSignInAsync(model.EmailAddress, model.Password, false, false);

                if (!result.Succeeded)
                    throw new Exception($"Failed to sign user '{model.EmailAddress}'.");

                _returnDictionary["Success"] = true;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public void SignOutAsync()
        {
            _signInManager.SignOutAsync().Wait();
        }
    }
}
