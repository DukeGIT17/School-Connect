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
        private Dictionary<string, object> _returnDictionary;
        
        public SignInService(ISignInRepo signInRepository, PasswordValidator<IdentityUser> passwordValidator)
        {
            _signInRepository = signInRepository;
            _passwordValidator = passwordValidator;
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

                _passwordValidator.ValidateAsync();
                if ()
            }
        }

        Task ISignInService.SignInAsync(string email, string password)
        {
            throw new NotImplementedException();
        }
    }
}
