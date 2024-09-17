using Microsoft.AspNetCore.Identity;
using SchoolConnect_DomainLayer.Data;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_RepositoryLayer.Interfaces;

namespace SchoolConnect_RepositoryLayer.Repositories
{
    public class SignInRepository : ISignInRepo
    {
        private readonly SignInManager<CustomIdentityUser> _signInManager;
        private readonly SchoolConnectDbContext _context;
        private Dictionary<string, object> _returnDictionary;

        public SignInRepository(SignInManager<CustomIdentityUser> signInManager, SchoolConnectDbContext context)
        {
            _signInManager = signInManager;
            _context = context;
            _returnDictionary = [];
        }

        public async Task<Dictionary<string, object>> SignInAsync(LoginModel model)
        {
            _returnDictionary = [];
            try
            {
                var user = await _signInManager.UserManager.FindByEmailAsync(model.EmailAddress);

                if (user == null)
                {
                    user = await _signInManager.UserManager.FindByNameAsync(model.EmailAddress);

                    _ = user ?? throw new Exception($"Could not find user with the email {model.EmailAddress}");
                }

                // TODO - Don't forget to check for the existence of an actor with the provided email.

                var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

                if (!result.Succeeded)
                    throw new Exception("Incorrect Password");

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

        public async Task<Dictionary<string, object>> SetNewPasswordAsync(LoginModel model)
        {
            _returnDictionary = [];
            try
            {
                var admin = _context.SystemAdmins.FirstOrDefault(x => x.EmailAddress == model.EmailAddress);
                if (admin == null) throw new Exception($"Actor with the email {model.EmailAddress} was not found.");


                var user = await _signInManager.UserManager.FindByEmailAsync(model.EmailAddress);
                if (user == null) throw new Exception($"User with the email {model.EmailAddress} was not found.");


                if (_signInManager.UserManager.CheckPasswordAsync(user, model.Password).Result)
                    throw new Exception("The old password you entered is incorrect.");

                var result = await _signInManager.UserManager.ChangePasswordAsync(user, model.Password, model.NewPassword!);
                if (!result.Succeeded) throw new Exception(result.Errors.First().Description);

                admin.Password = model.Password;
                _context.Update(admin);
                _context.SaveChanges();

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
