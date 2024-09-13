using SchoolConnect_DomainLayer.Models;
using SchoolConnect_DomainLayer.Data;
using SchoolConnect_RepositoryLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace SchoolConnect_RepositoryLayer.Repositories
{
    public class SystemAdminRepository : ISysAdmin
    {
        private readonly SchoolConnectDbContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        private Dictionary<string, object> _returnDictionary;

        public SystemAdminRepository(SchoolConnectDbContext context, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _returnDictionary = [];
            _signInManager = signInManager;
        }

        public async Task<Dictionary<string, object>> CreateAdmin(SysAdmin systemAdmin)
        {
            _returnDictionary = [];
            try
            {
                var admin = _context.SystemAdmins.FirstOrDefault(a => a.StaffNr == systemAdmin.StaffNr);

                if (admin != null)
                {
                    _returnDictionary.Add("Success", false);
                    _returnDictionary.Add("ErrorMessage", $"A system admin containing the staff number {systemAdmin.StaffNr} already exists.");
                    return _returnDictionary;
                }

                var user = await _signInManager.UserManager.FindByEmailAsync(systemAdmin.EmailAddress);

                if (user != null)
                {
                    _returnDictionary.Add("Success", false);
                    _returnDictionary.Add("ErrorMessage", $"A user with the email address {systemAdmin.EmailAddress} already exists.");
                    return _returnDictionary;
                }

                user = new IdentityUser
                {
                    Email = systemAdmin.EmailAddress,
                    UserName = systemAdmin.EmailAddress,
                    PhoneNumber = systemAdmin.PhoneNumber.ToString(),
                };

                var signInResult = await _signInManager.UserManager.CreateAsync(user, systemAdmin.Password);

                if (!signInResult.Succeeded)
                {
                    _returnDictionary.Add("Success", false);
                    _returnDictionary.Add("ErrorMessage", $"Failed to create an account for {systemAdmin.EmailAddress}. ({signInResult.Errors})");
                    return _returnDictionary;
                }

                var identityResult = await _signInManager.UserManager.AddToRoleAsync(user, systemAdmin.Role);

                if (!identityResult.Succeeded)
                {
                    var idResult = await _signInManager.UserManager.DeleteAsync(user);
                    if (!idResult.Succeeded)
                    {
                        _returnDictionary.Add("Success", false);
                        _returnDictionary.Add("ErrorMessage", $"CATASTROPHIC ERROR!!! A user '{systemAdmin.EmailAddress}' now exists without an assigned Role. ERROR!! ERROR!!.");
                        return _returnDictionary;
                    }

                    _returnDictionary.Add("Success", false);
                    _returnDictionary.Add("ErrorMessage", $"Something went wrong when assigning user '{systemAdmin.EmailAddress}' to specified role.");
                    return _returnDictionary;
                }

                _context.Add(systemAdmin);
                _context.SaveChanges();

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

        public async Task<Dictionary<string, object>> GetAdminById(long sysAdminId)
        {
            _returnDictionary = [];
            SysAdmin? admin;
            try
            {
                admin = await _context.SystemAdmins.FirstOrDefaultAsync(a => a.Id == sysAdminId);

                if (admin == null)
                {
                    _returnDictionary.Add("Success", false);
                    _returnDictionary.Add("ErrorMessage", $"Admin with the specified Id of {sysAdminId} doesn't exist within the database.");
                    return _returnDictionary;
                }

                _returnDictionary.Add("Success", true);
                _returnDictionary.Add("Result", admin);
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary.Add("Success", false);
                _returnDictionary.Add("ErrorMessage", ex.Message);
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetAdminByStaffNr(long staffNr)
        {
            _returnDictionary = [];
            SysAdmin? admin;
            try
            {
                admin = await _context.SystemAdmins.FirstOrDefaultAsync(a => a.StaffNr == staffNr);

                if (admin == null)
                {
                    _returnDictionary.Add("Success", false);
                    _returnDictionary.Add("ErrorMessage", $"Admin with the specified staff number of {staffNr} doesn't exist within the database.");
                    return _returnDictionary;
                }

                _returnDictionary.Add("Success", true);
                _returnDictionary.Add("Result", admin);
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary.Add("Success", false);
                _returnDictionary.Add("ErrorMessage", ex.Message);
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> Update(SysAdmin sysAdmin)
        {
            _returnDictionary = [];
            SysAdmin? admin;
            try
            {
                admin = await _context.SystemAdmins.FirstOrDefaultAsync(a => a.Id == sysAdmin.Id)
                   ?? await _context.SystemAdmins.FirstOrDefaultAsync(a => a.StaffNr == sysAdmin.StaffNr);

                if (admin == null)
                {
                    _returnDictionary.Add("Success", false);
                    _returnDictionary.Add("ErrorMessage", "Admin with the specified Id or staff number wasn't found.");
                    return _returnDictionary;
                }

                _context.Update(sysAdmin);
                _context.SaveChanges();
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
    }
}
