using Microsoft.EntityFrameworkCore;
using SchoolConnect_DomainLayer.Data;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_RepositoryLayer.Interfaces;

namespace SchoolConnect_RepositoryLayer.Repositories
{
    public class SchoolRepository : ISchool
    {
        private readonly SchoolConnectDbContext _context;
        private Dictionary<string, object> _returnDictionary;

        public SchoolRepository(SchoolConnectDbContext context)
        {
            _context = context;
            _returnDictionary = [];
        }

        public async Task<Dictionary<string, object>> RegisterSchoolAsync(School newSchool)
        {
            try
            {
                var schools = await _context.Schools.ToListAsync();
                var school = schools.FirstOrDefault(x => x.SystemAdminId == newSchool.SystemAdminId);
                if (school != null)
                {
                    var admin = await _context.SystemAdmins.FirstOrDefaultAsync(a => a.Id == newSchool.SystemAdminId);
                    if (admin == null)
                        throw new Exception($"An admin can only register a single school, admin with the ID {school.SystemAdminId} already has a school associated with them in the system.");

                    throw new Exception($"An admin can only register a single school, admin {admin.Name} {admin.Surname} with the ID {newSchool.SystemAdminId} already has a school associated with them in the system.");
                }

                school = schools.FirstOrDefault(x => x.EmisNumber == newSchool.EmisNumber);
                if (school != null)
                    throw new Exception($"A school possessing the emis number, '{newSchool.EmisNumber}', already exists within the database.");

                newSchool.SchoolAddress.SchoolID = schools.Count + 1;

                await _context.AddAsync(newSchool);
                await _context.SaveChangesAsync();

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

        public async Task<Dictionary<string, object>> GetSchoolsAsync()
        {
            try
            {
                var schools = await _context.Schools.ToListAsync();
                if (schools == null || schools.Count < 1) throw new Exception("No schools in the database.");
                _returnDictionary["Success"] = true;
                _returnDictionary["Result"] = schools;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public Task<Dictionary<string, object>> GetSchoolsByChild(long childId)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, object>> GetSchoolsById(long schoolId)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, object>> GetSchoolsByName(string schoolName)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, object>> RemoveSchool(long emisNumber, long schoolId = -1)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, object>> UpdateSchoolInfo(School school)
        {
            throw new NotImplementedException();
        }
    }
}
