using Microsoft.EntityFrameworkCore;
using SchoolConnect_DomainLayer.Data;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_RepositoryLayer.CommonAction;
using SchoolConnect_RepositoryLayer.Interfaces;

namespace SchoolConnect_RepositoryLayer.Repositories
{
    public class LearnerRepository : ILearner
    {
        private readonly SchoolConnectDbContext _context;
        private readonly ISignInRepo _signInRepo;
        private Dictionary<string, object> _returnDictionary;

        public LearnerRepository(SchoolConnectDbContext context, ISignInRepo signInRepo)
        {
            _context = context;
            _signInRepo = signInRepo;
            _returnDictionary = [];
        }

        public async Task<Dictionary<string, object>> CreateAsync(Learner learner)
        {
            try
            {
                var school = await _context.Schools.FirstOrDefaultAsync(s => s.Id == learner.SchoolID);
                if (school == null)
                    throw new($"Something went wrong, could not find a school with the ID {learner.SchoolID}.");

                learner.LearnerSchoolNP = school;

                List<Parent> parentsToAttach = [];
                List<string> returnErrors = [];

                var result = await _context.Learners.FirstOrDefaultAsync(l => l.IdNo == learner.IdNo);
                if (result != null)
                    throw new($"A learner with the specified ID number already exists.");
                var parents = await _context.Parents.ToListAsync();
                foreach (var parent in learner.Parents)
                {
                    var existingParent = parents.FirstOrDefault(p => p.IdNo == parent.ParentIdNo);
                    if (existingParent != null)
                    {
                        _context.Parents.Attach(existingParent);
                        parentsToAttach.Add(existingParent);
                    }

                    if (existingParent == null && parent.Parent == null)
                        throw new($"An attempt was made to create a learner with a parent possessing the ID number '{parent.ParentIdNo}'. " +
                            $"However, a parent with this ID number doesn't yet exist within the database. If you wish to register a new parent along " +
                            $"with this learner, please provide the relevant parent data.");
                }

                foreach (var parent in parentsToAttach)
                {
                    var p = learner.Parents.FirstOrDefault(p => p.ParentIdNo == parent.IdNo);
                    if (p != null)
                        p.Parent = parent;
                }

                foreach (var parent in learner.Parents)
                {
                    _returnDictionary = await _signInRepo.CreateUserAccountAsync(parent.Parent!.EmailAddress, parent.Parent.Role, parent.Parent.PhoneNumber.ToString());
                    if (!(bool)_returnDictionary["Success"])
                        throw new(_returnDictionary["ErrorMessage"] as string);
                }

                await _context.AddAsync(learner);
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

        public async Task<Dictionary<string, object>> GetById(long learnerId)
        {
            try
            {
                return await CommonActions.GetActorById(learnerId, new Learner(), _context);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = true;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public Task<Dictionary<string, object>> GetByIdNo(long learnerIdNo)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, object>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, object>> GetClass(string classId)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, object>> Remove(long learnerId)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, object>> RemoveClass(string classId)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, object>> UpdateLearner(Learner learner)
        {
            throw new NotImplementedException();
        }
    }
}
