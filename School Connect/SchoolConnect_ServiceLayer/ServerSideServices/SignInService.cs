using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_RepositoryLayer.Interfaces;
using SchoolConnect_ServiceLayer.IServerSideServices;

namespace SchoolConnect_ServiceLayer.ServerSideServices
{
    public class SignInService : ISignInService
    {
        private readonly ISignInRepo _signInRepository;
        private readonly PasswordValidator<CustomIdentityUser> _passwordValidator;
        private readonly UserManager<CustomIdentityUser> _userManager;
        private Dictionary<string, object> _returnDictionary;

        public SignInService(ISignInRepo signInRepository, PasswordValidator<CustomIdentityUser> passwordValidator, UserManager<CustomIdentityUser> userManager)
        {
            _signInRepository = signInRepository;
            _passwordValidator = passwordValidator;
            _userManager = userManager;
            _returnDictionary = [];
        }

        private static bool CheckEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            return Regex.IsMatch(email, pattern);
        }
        private bool CheckPassword(string password)
        {
            var userManager = _userManager;
            CustomIdentityUser user = new();
            var identityResult = _passwordValidator.ValidateAsync(userManager, user, password).Result;
            return identityResult.Succeeded;
        }
        public async Task<Dictionary<string, object>> SignInAsync(LoginModel model)
        {
            _returnDictionary = [];
            try
            {
                if (!CheckEmail(model.EmailAddress))
                    throw new Exception($"{model.EmailAddress} is an invalid email address.");

                if (!CheckPassword(model.Password))
                    throw new Exception("Please enter a valid password");

                _returnDictionary = await _signInRepository.SignInAsync(model);
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }
        public async Task<Dictionary<string, object>> SetNewPasswordAsync(LoginModel model)
        {
            _returnDictionary = [];
            try
            {
                if (!CheckEmail(model.EmailAddress)) throw new Exception($"{model.EmailAddress} is an invalid email address.");
                if (!CheckPassword(model.Password)) throw new Exception("Please enter a valid password");

                _returnDictionary = await _signInRepository.SetNewPasswordAsync(model);
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
