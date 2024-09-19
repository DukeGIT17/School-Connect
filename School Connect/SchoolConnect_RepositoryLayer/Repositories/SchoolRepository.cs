using Microsoft.EntityFrameworkCore;
using SchoolConnect_DomainLayer.Data;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_RepositoryLayer.Interfaces;

namespace SchoolConnect_RepositoryLayer.Repositories
{
    public class SchoolRepository : ISchool
    {
        private readonly SchoolConnectDbContext _context;
        private Dictionary<string, object>? _returnDictionary;

        public SchoolRepository(SchoolConnectDbContext context)
        {
            _context = context;
        }

        public async Task<Dictionary<string, object>> RegisterSchoolAsync(School school)
        {
            _returnDictionary = [];
            School? result;
            try
            {
                _returnDictionary.Clear();
                result = await _context.Schools.FirstOrDefaultAsync(x => x.EmisNumber == school.EmisNumber);

                if (result != null)
                {

                    _returnDictionary.Add("Success", false);
                    _returnDictionary.Add("ErrorMessage", $"A school possessing the emis number: '{school.EmisNumber}', already exists within the database.");
                    return _returnDictionary;
                }

                await _context.AddAsync(school);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _returnDictionary.Add("Success", false);
                _returnDictionary.Add("ErrorMessage", ex.Message);
                return _returnDictionary;
            }

            _returnDictionary.Add("Success", true);
            return _returnDictionary;
        }

        public async Task<Dictionary<string, object>> GetSchoolsAsync()
        {
            _returnDictionary = [];
            List<School>? schools;
            List<SysAdmin>? systemAdmins;
            try
            {
                _returnDictionary.Clear();
                schools = await _context.Schools.ToListAsync();
                systemAdmins = await _context.SystemAdmins.ToListAsync();

                foreach (var school in schools)
                {
                    throw new Exception($"{systemAdmins.FirstOrDefault(s => s.Id == school.Id).Name} - System Admin ID in School {school.Id} - School {school.Name}");
                    school.SchoolSysAdminNP = systemAdmins.FirstOrDefault(s => s.Id == school.Id);
                }

                if (schools == null || schools.Count == 0)
                {
                    _returnDictionary.Add("Success", false);
                    _returnDictionary.Add("ErrorMessage", "Fetching schools resolved to a null or empty list.");
                    return _returnDictionary;
                }
            }
            catch (Exception ex)
            {
                _returnDictionary.Add("Success", false);
                _returnDictionary.Add("ErrorMessage", ex.Message);
                return _returnDictionary;
            }

            _returnDictionary.Add("Success", true);
            _returnDictionary.Add("Result", schools);
            return _returnDictionary;
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
