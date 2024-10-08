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
        private readonly SignInManager<CustomIdentityUser> _signInManager;
        private Dictionary<string, object> _returnDictionary;

        public SystemAdminRepository(SchoolConnectDbContext context, SignInManager<CustomIdentityUser> signInManager)
        {
            _context = context;
            _returnDictionary = [];
            _signInManager = signInManager;
        }

        public async Task<Dictionary<string, object>> GetAdminByIdAsync(long sysAdminId)
        {
            try
            {
                _returnDictionary = await CommonActions.GetActorById(sysAdminId, new SysAdmin(), _context);
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                var admin = _returnDictionary["Result"] as SysAdmin;
                var school = await _context.Schools.FirstOrDefaultAsync(s => s.Id == admin!.SysAdminSchoolNP!.Id);
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
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

        public async Task<Dictionary<string, object>> Update(SysAdmin sysAdmin)
        {
            _returnDictionary = [];
            SysAdmin? admin;
            try
            {
                admin = await _context.SystemAdmins.AsNoTracking().FirstOrDefaultAsync(a => a.StaffNr == sysAdmin.StaffNr || a.Id == sysAdmin.Id) ;

                if (admin == null)
                    throw new Exception("No admin with the specified Id and staff numbers was found.");

                _context.Update(sysAdmin);
                _context.SaveChanges();

                _returnDictionary["Success"] = true;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = true;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }
    }
}
