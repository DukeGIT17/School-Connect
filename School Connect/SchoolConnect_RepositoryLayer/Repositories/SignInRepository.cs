using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

                string role = _signInManager.UserManager.GetRolesAsync(user).Result.First();
                switch (role)
                {
                    case "System Admin":
                        SysAdmin? admin = await _context.SystemAdmins.FirstOrDefaultAsync(s => s.EmailAddress == user.Email);
                        if (admin == null) throw new Exception($"Actor {model.EmailAddress}'s data was not found");
                        break;
                    case "Principal":
                        Principal? principal = await _context.Principals.FirstOrDefaultAsync(p => p.EmailAddress == user.Email);
                        if (principal == null) throw new Exception($"Actor {model.EmailAddress}'s data was not found");
                        break;
                    case "Teacher":
                        Teacher? teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.EmailAddress == user.Email);
                        if (teacher == null) throw new Exception($"Actor {model.EmailAddress}'s data was not found");
                        break;
                    case "Parent":
                        Parent? parent = await _context.Parents.FirstOrDefaultAsync(p => p.EmailAddress == user.Email);
                        if (parent == null) throw new Exception($"Actor {model.EmailAddress}'s data was not found.");
                    break;
                    default:
                        throw new Exception($"Error! Something went when determining user's role. Role passed in {role}");
                    
                }

                var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                if (!result.Succeeded) throw new Exception("Incorrect Password.");

                _returnDictionary["Success"] = true;
                _returnDictionary["ResetPassword"] = user.ResetPassword;
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
                var admins = await _context.SystemAdmins.ToListAsync();
                var admin = admins.FirstOrDefault(x => x.EmailAddress.Equals(model.EmailAddress, StringComparison.OrdinalIgnoreCase));                
                if (admin == null) throw new Exception($"Actor with the email {model.EmailAddress} was not found.");


                var user = await _signInManager.UserManager.FindByEmailAsync(model.EmailAddress);
                if (user == null) throw new Exception($"User with the email {model.EmailAddress} was not found.");

                bool isCorrect = _signInManager.UserManager.CheckPasswordAsync(user, model.Password).Result;
                if (!isCorrect)
                    throw new Exception("The old password you entered is incorrect.");

                var result = await _signInManager.UserManager.ChangePasswordAsync(user, model.Password, model.NewPassword!);
                if (!result.Succeeded) throw new Exception(result.Errors.First().Description);

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
