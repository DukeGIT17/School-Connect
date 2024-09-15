using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using SchoolConnect_RepositoryLayer.Interfaces;
using SchoolConnect_ServiceLayer.ISystemAdminServices;

namespace SchoolConnect_ServiceLayer.SystemAdminServices
{
    public class SignInService : ISignInService
    {
        private readonly ISignInRepo _signInRepository;
        private readonly PasswordValidator<IdentityUser> _passwordValidator;
        private readonly UserManager<IdentityUser> _userManager;
        private Dictionary<string, object> _returnDictionary;
        
        public SignInService(ISignInRepo signInRepository, PasswordValidator<IdentityUser> passwordValidator, UserManager<IdentityUser> userManager)
        {
            _signInRepository = signInRepository;
            _passwordValidator = passwordValidator;
            _userManager = userManager;
            _returnDictionary = [];
        }

        public async Task<Dictionary<string, object>> SignInAsync(string email, string password)
        {
            _returnDictionary = [];
            try
            {
                string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
                bool isValid = Regex.IsMatch(email, pattern);

                if (!isValid)
                {
                    throw new Exception($"{email} is an invalid email address.");
                }

                var userManager = _userManager;
                IdentityUser user = new();
                var identityResult = await _passwordValidator.ValidateAsync(userManager, user, password);

                if (!identityResult.Succeeded)
                    throw new Exception("Please enter a valid password");

                _returnDictionary = await _signInRepository.SignInAsync(email, password);
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        void ISignInService.SignOutAsync()
        {
            _signInRepository.SignOutAsync();
        }
    }
}
