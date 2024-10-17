using SchoolConnect_DomainLayer.Models;
using SchoolConnect_DomainLayer.Data;
using SchoolConnect_RepositoryLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SchoolConnect_RepositoryLayer.CommonAction;

namespace SchoolConnect_RepositoryLayer.Repositories
{
    public class SystemAdminRepository : ISysAdmin
    {
        private readonly SchoolConnectDbContext _context;
        private readonly ISignInRepo _signInRepo;
        private Dictionary<string, object> _returnDictionary;

        public SystemAdminRepository(SchoolConnectDbContext context, ISignInRepo signInRepo)
        {
            _context = context;
            _returnDictionary = [];
            _signInRepo = signInRepo;
        }

        public async Task<Dictionary<string, object>> GetAdminByIdAsync(long sysAdminId)
        {
            try
            {
                var admin = await _context.SystemAdmins.Include(s => s.SysAdminSchoolNP).ThenInclude(s => s.SchoolAddress).FirstOrDefaultAsync(s => s.Id == sysAdminId);
                if (admin is null) throw new($"Could not find an actor with the ID {sysAdminId}.");

                _returnDictionary["Success"] = true;
                _returnDictionary["Result"] = admin;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message + "\nInner Exception: " + ex.InnerException;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetAdminByStaffNrAsync(string staffNr)
        {
            _returnDictionary = [];
            SysAdmin? admin;
            try
            {
                admin = await _context.SystemAdmins.FirstOrDefaultAsync(a => a.StaffNr == staffNr);

                if (admin == null)
                    throw new Exception($"Could not find admin with the ID: {staffNr}");


                _returnDictionary["Success"] = true;
                _returnDictionary["Result"] = admin;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> UpdateAsync(SysAdmin sysAdmin)
        {
            try
            {
                var admin = await _context.SystemAdmins.AsNoTracking().FirstOrDefaultAsync(a => a.Id == sysAdmin.Id);
                if (admin == null) throw new("No admin with the specified Id and staff numbers was found.");

                if (sysAdmin.EmailAddress != admin.EmailAddress)
                {
                    _returnDictionary = await _signInRepo.ChangeEmailAsync(admin.EmailAddress, sysAdmin.EmailAddress);
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                }
                
                if (sysAdmin.PhoneNumber != admin.PhoneNumber)
                {
                    _returnDictionary = await _signInRepo.ChangePhoneNumberAsync(admin.PhoneNumber.ToString(), sysAdmin.PhoneNumber.ToString(), sysAdmin.EmailAddress);
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                }

                _context.Update(sysAdmin);
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
    }
}
