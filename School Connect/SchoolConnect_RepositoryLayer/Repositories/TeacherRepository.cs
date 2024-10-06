using SchoolConnect_RepositoryLayer.Interfaces;
using SchoolConnect_DomainLayer.Data;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_RepositoryLayer.CommonAction;

namespace SchoolConnect_RepositoryLayer.Repositories
{
    public class TeacherRepository : ITeacher
    {
        private readonly SchoolConnectDbContext _context;
        private readonly ISignInRepo _signInRepo;
        private readonly IGroup _groupRepo;
        private Dictionary<string, object> _returnDictionary;

        public TeacherRepository(SchoolConnectDbContext context, ISignInRepo signInRepo, IGroup groupRepo)
        {
            _context = context;
            _signInRepo = signInRepo;
            _groupRepo = groupRepo;
            _returnDictionary = [];
        }

        public async Task<Dictionary<string, object>> CreateAsync(Teacher teacher)
        {
            try
            {
                var result = _context.Teachers.FirstOrDefault(t => t.StaffNr == teacher.StaffNr);
                if (result != null) throw new($"A teacher with the specified staff number '{teacher.StaffNr}' already exists within the database.");

                var school = _context.Schools.FirstOrDefault(s => s.Id == teacher.SchoolID) 
                    ?? throw new($"A school with the Id '{teacher.SchoolID}' does not exist. Has this school been registered yet?");

                teacher.TeacherSchoolNP = school;

                _returnDictionary = await _groupRepo.AddActorToGroup(teacher.StaffNr, teacher.SchoolID, "All");
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                _returnDictionary = await _signInRepo.CreateUserAccountAsync(teacher.EmailAddress, teacher.Role, teacher.PhoneNumber.ToString());
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                await _context.AddAsync(teacher);
                await _context.SaveChangesAsync();

                _returnDictionary["Success"] = true;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message + "\nInner Exception: " + ex.InnerException;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetById(long teacherId)
        {
            try
            {
                return await CommonActions.GetActorById(teacherId, new Teacher(), _context);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = true;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public Task<Dictionary<string, object>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, object>> GetTeacherByClass(string classId)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, object>> Remove(long teacherId)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, object>> Update(Teacher teacher)
        {
            throw new NotImplementedException();
        }
    }
}
