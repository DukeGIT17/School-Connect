using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolConnect_DomainLayer.Data;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_RepositoryLayer.Interfaces;
using System.Collections.Frozen;

namespace SchoolConnect_RepositoryLayer.Repositories
{
    public class SignInRepository : ISignInRepo
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SignInManager<CustomIdentityUser> _signInManager;
        private readonly SchoolConnectDbContext _context;
        private Dictionary<string, object> _returnDictionary;

        public SignInRepository(SignInManager<CustomIdentityUser> signInManager, SchoolConnectDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _signInManager = signInManager;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _returnDictionary = [];
        }

        private async Task<Dictionary<string, object>> DetermineActorTypeAsync(CustomIdentityUser user, LoginModel model)
        {
            try
            {
                string role = _signInManager.UserManager.GetRolesAsync(user).Result.First();
                switch (role)
                {
                    case "System Admin":
                        SysAdmin? admin = await _context.SystemAdmins.FirstOrDefaultAsync(s => s.EmailAddress == user.Email);
                        if (admin == null) throw new Exception($"Actor {model.EmailAddress}'s data was not found");
                        _returnDictionary["ActorID"] = admin.Id;
                        break;
                    case "Principal":
                        Principal? principal = await _context.Principals.FirstOrDefaultAsync(p => p.EmailAddress == user.Email);
                        if (principal == null) throw new Exception($"Actor {model.EmailAddress}'s data was not found");
                        _returnDictionary["ActorID"] = principal.Id;
                        break;
                    case "Teacher":
                        Teacher? teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.EmailAddress == user.Email);
                        if (teacher == null) throw new Exception($"Actor {model.EmailAddress}'s data was not found");
                        _returnDictionary["ActorID"] = teacher.Id;
                        break;
                    case "Parent":
                        Parent? parent = await _context.Parents.FirstOrDefaultAsync(p => p.EmailAddress == user.Email);
                        if (parent == null) throw new Exception($"Actor {model.EmailAddress}'s data was not found.");
                        _returnDictionary["ActorID"] = parent.Id;
                        break;
                    default:
                        throw new Exception($"Error! Something went when determining user's role. Role passed in {role}");

                }

                _returnDictionary["Success"] = true;
                _returnDictionary["Role"] = role;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> CreateUserAccountAsync(string email, string role, string? phoneNumber = null)
        {
            try
            {
                string password = "Default12345!";
                var user = new CustomIdentityUser
                {
                    Email = email,
                    UserName = email,
                    PhoneNumber = phoneNumber,
                    ResetPassword = true
                };

                if (await _signInManager.UserManager.FindByEmailAsync(email) == null)
                {
                    var result = await _signInManager.UserManager.CreateAsync(user, password);
                    if (!result.Succeeded)
                        throw new($"Failed to create account for user '{email}'. {result.Errors.First()} - plus {result.Errors.Count()} more");

                    result = await _signInManager.UserManager.AddToRoleAsync(user, role);
                    if (!result.Succeeded) throw new($"Failed to add user to the {role} role. Action may have to be taken to clear malformed or incomplete user data in the database.");

                    _returnDictionary["Success"] = true;
                    return _returnDictionary;
                }

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

        public async Task<Dictionary<string, object>> SignInAsync(LoginModel model)
        {
            try
            {
                var user = await _signInManager.UserManager.FindByEmailAsync(model.EmailAddress);
                if (user == null)  throw new($"Could not find user with the email {model.EmailAddress}");

                _returnDictionary = await DetermineActorTypeAsync(user, model);
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                var role = _returnDictionary["Role"] as string ?? throw new("Something went wrong. No role returned.");

                var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                if (!result.Succeeded) throw new("Incorrect Password.");

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
            try
            {
                var user = await _signInManager.UserManager.FindByEmailAsync(model.EmailAddress);
                if (user == null) throw new($"User with the email {model.EmailAddress} was not found.");

                _returnDictionary = await DetermineActorTypeAsync(user, model);
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                bool isCorrect = _signInManager.UserManager.CheckPasswordAsync(user, model.Password).Result;
                if (!isCorrect) throw new("The old password you entered is incorrect.");

                var result = await _signInManager.UserManager.ChangePasswordAsync(user, model.Password, model.NewPassword!);
                if (!result.Succeeded) throw new(result.Errors.First().Description);
                user.ResetPassword = false;

                result = await _signInManager.UserManager.UpdateAsync(user);
                if (!result.Succeeded) throw new(result.Errors.First().Description);

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

        public void IsSignedIn()
        {
            var val = _httpContextAccessor.HttpContext.User;
            var value = _signInManager.IsSignedIn(val);
            Console.WriteLine(value);
        }

        public void SignOutAsync()
        {
            _signInManager.SignOutAsync().Wait();
        }

        public async Task<Dictionary<string, object>> RemoveUserAccountAsync(string email, string role)
        {
            try
            {
                var user = await _signInManager.UserManager.FindByEmailAsync(email);
                if (user == null)
                    throw new($"User account with the email {email} could not be found.");

                if (_signInManager.UserManager.RemoveFromRoleAsync(user, role).Result.Succeeded)
                {
                    SignOutAsync();
                    if (!_signInManager.UserManager.DeleteAsync(user).Result.Succeeded)
                        throw new($"Error. Could not delete user {email}.");
                }

                throw new("Something went wrong while deleting user {email}.");
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> ChangeEmailAddressAsync(string oldEmail, string newEmail)
        {
            try
            {
                var user = await _signInManager.UserManager.FindByEmailAsync(oldEmail);
                if (user is null) throw new($"Could not find a user with the email address {oldEmail}.");
                var token = await _signInManager.UserManager.GenerateChangeEmailTokenAsync(user, newEmail);

                var result = await _signInManager.UserManager.ChangeEmailAsync(user, newEmail, token);
                if (!result.Succeeded)
                {
                    string errors = "";
                    result.Errors.ToList().ForEach(x => errors += $"{x}\n");
                    throw new(errors);
                }

                user.UserName = newEmail;
                result = await _signInManager.UserManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    string errors = "";
                    result.Errors.ToList().ForEach(x => errors += $"{x}\n");
                    throw new(errors);
                }

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
        
        public async Task<Dictionary<string, object>> ChangePhoneNumberAsync(string oldPhoneNumber, string newPhoneNumber, string email)
        {
            try
            {
                var user = await _signInManager.UserManager.FindByEmailAsync(email);
                if (user is null) throw new($"Could not find a user with the email address {email}.");
                var token = await _signInManager.UserManager.GenerateChangePhoneNumberTokenAsync(user, newPhoneNumber);

                var result = await _signInManager.UserManager.ChangePhoneNumberAsync(user, newPhoneNumber, token);
                if (!result.Succeeded)
                {
                    string errors = "";
                    result.Errors.ToList().ForEach(x => errors += $"{x}\n");
                    throw new(errors);
                }

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
    }
}
