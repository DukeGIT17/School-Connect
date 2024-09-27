using SchoolConnect_DomainLayer.Data;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_RepositoryLayer.Interfaces;

namespace SchoolConnect_RepositoryLayer.Repositories
{
    public class LearnerRepository : ILearner
    {
        private readonly SchoolConnectDbContext _context;
        private Dictionary<string, object> _returnDictionary;

        public LearnerRepository(SchoolConnectDbContext context)
        {
            _context = context;
            _returnDictionary = [];
        }

        public async Task<Dictionary<string, object>> CreateAsync(Learner learner)
        {
            try
            {
                var result = _context.Learners.FirstOrDefault(l => l.IdNo == learner.IdNo);
                if (result != null)
                    throw new Exception($"A learner with the specified ID number already exists.");

                await _context.AddAsync(learner);
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

        public Task<Dictionary<string, object>> Get(long learnerId)
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
