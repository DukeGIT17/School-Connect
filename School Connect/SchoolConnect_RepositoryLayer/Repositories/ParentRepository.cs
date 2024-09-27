using SchoolConnect_DomainLayer.Data;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_RepositoryLayer.Interfaces;

namespace SchoolConnect_RepositoryLayer.Repositories
{
    public class ParentRepository : IParent
    {
        private readonly SchoolConnectDbContext _context;
        private Dictionary<string, object> _returnDictionary;

        public ParentRepository(SchoolConnectDbContext context)
        {
            _context = context;
            _returnDictionary = [];
        }

        public async Task<Dictionary<string, object>> CreateAsync(Parent parent)
        {
            try
            {
                var result = _context.Parents.FirstOrDefault(p => p.IdNo == parent.IdNo);
                if (result != null) throw new("A parent with the specified Id number already exists.");

                foreach (var child in parent.Children)
                {
                    var learner = _context.Learners.FirstOrDefault(l => l.Id == child.LearnerID) 
                        ?? throw new Exception($"A learner with the ID '{child.LearnerID}' does not exist.");
                }

                await _context.AddAsync(parent);
                await _context.SaveChangesAsync();

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
