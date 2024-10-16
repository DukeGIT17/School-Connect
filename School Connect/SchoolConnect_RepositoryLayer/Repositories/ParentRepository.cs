using SchoolConnect_DomainLayer.Data;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_RepositoryLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using static SchoolConnect_RepositoryLayer.CommonAction.CommonActions;

namespace SchoolConnect_RepositoryLayer.Repositories
{
    public class ParentRepository : IParent
    {
        private readonly SchoolConnectDbContext _context;
        private readonly ISignInRepo _signInRepo;
        private readonly IGroupRepo _groupRepo;
        private Dictionary<string, object> _returnDictionary;

        public ParentRepository(SchoolConnectDbContext context, ISignInRepo signInRepo, IGroupRepo groupRepo)
        {
            _context = context;
            _signInRepo = signInRepo;
            _groupRepo = groupRepo;
            _returnDictionary = [];
        }

        public async Task<Dictionary<string, object>> CreateAsync(Parent parent)
        {
            try
            {
                var result = _context.Parents.FirstOrDefault(p => p.IdNo == parent.IdNo);
                if (result != null) throw new("A parent with the specified Id number already exists.");

                List<Learner> learnersToAttach = [];
                List<LearnerParent> nonExistentLearner = [];
                List<string> errors = [];
                foreach (var child in parent.Children!)

                {
                    var learner = await _context.Learners.FirstOrDefaultAsync(l => l.IdNo == child.LearnerIdNo);
                    if (learner == null)
                    {
                        nonExistentLearner.Add(child);
                        parent.Children.Remove(child);
                        errors.Add($"A learner with the ID number {child.LearnerIdNo} does not exist, please create an account for them " +
                            "and link them to this parent.");
                    }

                    if (learner != null)
                    {
                        _context.Learners.Attach(learner);
                        learnersToAttach.Add(learner);
                    }
                }

                if (errors.Count >= parent.Children.Count)
                    throw new("All the learners provided do not exist within the database thus this new parent has no children to be linked to. " +
                        "Please provide valid ID numbers or create profiles for the learners first.");

                foreach (var learner in learnersToAttach)
                {
                    var lp = parent.Children.FirstOrDefault(l => l.LearnerIdNo == learner.IdNo);
                    if (lp != null)
                        lp.Learner = learner;
                }

                foreach (var learner in parent.Children)
                {
                    _returnDictionary = await _groupRepo.AddActorToGroup(parent.IdNo, learner.Learner!.SchoolID, "All");
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                }

                _returnDictionary = await _signInRepo.CreateUserAccountAsync(parent.EmailAddress, parent.Role, parent.PhoneNumber.ToString());
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                _returnDictionary["AdditionalInformation"] = errors;

                if (parent.ProfileImageFile is not null)
                {
                    _returnDictionary = SaveImage($"{parent.Name} {parent.Surname}", "Profile Images Folder/Parents", parent.ProfileImageFile);
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                    parent.ProfileImage = _returnDictionary["FileName"] as string;
                }
                else
                    parent.ProfileImage = "Default Pic.png";

                await _context.AddAsync(parent);
                await _context.SaveChangesAsync();

                _returnDictionary["Success"] = true;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = true;
                _returnDictionary["ErrorMessage"] = ex.Message + "\nInner Exception: " + ex.InnerException;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetById(long parentId)
        {
            try
            {
                return await GetActorById(parentId, new Parent(), _context);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = true;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public Task<Dictionary<string, object>> GetByIdNo(string parentIdNo)
        {
            throw new NotImplementedException();
        }
    }
}
