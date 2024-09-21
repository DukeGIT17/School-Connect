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

        public async Task<Dictionary<string, object>> RegisterSchoolAsync(School school)
        {
            try
            {
                //TODO: Implement restrictions to ensure an admin cannot register more than one school.
                var result = await _context.Schools.FirstOrDefaultAsync(x => x.EmisNumber == school.EmisNumber);
                if (result != null)
                    throw new Exception($"A school possessing the emis number: '{school.EmisNumber}', already exists within the database.");

                await _context.AddAsync(school);
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
