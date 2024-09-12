using SchoolConnect_DomainLayer.Models;
using SchoolConnect_DomainLayer.Data;
using SchoolConnect_RepositoryLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace SchoolConnect_RepositoryLayer.Repositories
{
    public class SystemAdminRepository : ISysAdmin
    {
        private readonly SchoolConnectDbContext _context;
        private Dictionary<string, object> _returnDictionary;

        public SystemAdminRepository(SchoolConnectDbContext context)
        {
            _context = context;
            _returnDictionary = [];
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
