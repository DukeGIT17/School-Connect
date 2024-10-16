using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using SchoolConnect_DomainLayer.Data;
using SchoolConnect_DomainLayer.Models;
using static SchoolConnect_RepositoryLayer.CommonAction.CommonActions;
using SchoolConnect_RepositoryLayer.Interfaces;

namespace SchoolConnect_RepositoryLayer.Repositories
{
    public class LearnerRepository(SchoolConnectDbContext context, ISignInRepo signInRepo, IGroupRepo groupRepo) : ILearner
    {
        private readonly SchoolConnectDbContext _context = context;
        private readonly ISignInRepo _signInRepo = signInRepo;
        private readonly IGroupRepo _groupRepo = groupRepo;
        private Dictionary<string, object> _returnDictionary = [];

        private async Task<Dictionary<string, object>> LearnerExistsAsync(Learner learner)
        {
            try
            {
                var result = await _context.Learners.FirstOrDefaultAsync(l => l.IdNo == learner.IdNo);
                if (result != null)
                    throw new($"Learner {learner.Name} registration failed. A learner with the specified ID number already exists.");

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
        private Dictionary<string, object> CheckAndAddParents(ref Learner learner, ref List<Parent> parents, ref List<Parent> parentsToAttach)
        {
            try
            {
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
        private Dictionary<string, object> AddClassDetailsToLearner(ref Learner learner)
        {
            try
            {
                bool found = false;
                foreach (var grade in learner.LearnerSchoolNP!.SchoolGradeNP!)
                {
                    var classCode = learner.ClassCode;
                    var cls = grade.Classes.FirstOrDefault(c => c.ClassDesignate == classCode);
                    if (cls != null)
                    {
                        learner.Class = cls;
                        learner.ClassID = cls.Id;
                        found = true;
                    }
                }

                _returnDictionary["Success"] = found;
                if (!found)
                    _returnDictionary["ErrorMessage"] = $"The class {learner.ClassCode} could not be found.";
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }
        public async Task<Dictionary<string, object>> BatchLoadLearnersFromExcel(string fileName)
        {
            try
            {
                var learners = new List<Learner>();
                var parents = new List<Parent>();
                var learnerIdNos = new List<long>();

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                string folderPath = @"C:\Users\KHAYE\OneDrive\Documents\WIL\";
                using (var stream = new FileStream(folderPath + fileName, FileMode.Open, FileAccess.Read))
                using (var package = new ExcelPackage(stream))
                {
                    var learnerWorksheet = package.Workbook.Worksheets[0];
                    var parentWorksheet = package.Workbook.Worksheets[1];
                    var rowCount = learnerWorksheet.Dimension.Rows;
                    var parentRowCount = parentWorksheet.Dimension.Rows;


                    for (int row = 2; row <= parentRowCount; row++)
                    {
                        var parent = new Parent
                        {
                            ProfileImage = parentWorksheet.Cells[row, 1].Value?.ToString(),
                            Name = parentWorksheet.Cells[row, 2].Value?.ToString(),
                            Surname = parentWorksheet.Cells[row, 3].Value?.ToString(),
                            Gender = parentWorksheet.Cells[row, 4].Value?.ToString(),
                            IdNo = parentWorksheet.Cells[row, 5].Value?.ToString(),
                            ParentType = parentWorksheet.Cells[row, 6].Value?.ToString(),
                            EmailAddress = parentWorksheet.Cells[row, 7].Value?.ToString(),
                            PhoneNumber = Convert.ToInt64(parentWorksheet.Cells[row, 8].Value),
                            Role = "Parent"
                        };

                        _returnDictionary = AttemptObjectValidation(parent);
                        if (!(bool)_returnDictionary["Success"])
                        {
                            var errors = _returnDictionary["Errors"] as List<string>;
                            string longErrorString = "";
                            foreach (var error in errors!)
                                longErrorString += error.ToString() + "\n";

                            throw new(longErrorString);
                        }

                        parents.Add(parent);
                        learnerIdNos.Add(Convert.ToInt64(parentWorksheet.Cells[row, 9].Value));
                    }

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var learner = new Learner
                        {
                            ProfileImage = learnerWorksheet.Cells[row, 1].Value?.ToString(),
                            Name = learnerWorksheet.Cells[row, 2].Value?.ToString(),
                            Surname = learnerWorksheet.Cells[row, 3].Value?.ToString(),
                            Gender = learnerWorksheet.Cells[row, 4].Value?.ToString(),
                            IdNo = learnerWorksheet.Cells[row, 5].Value?.ToString(),
                            ClassCode = learnerWorksheet.Cells[row, 6].Value?.ToString(),
                            Subjects = learnerWorksheet.Cells[row, 7].Value?.ToString()
                        .Trim('[', ']')
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(item => item.Trim(' ', '"', '\''))
                        .ToList(),
                            SchoolID = Convert.ToInt64(learnerWorksheet.Cells[row, 8].Value),
                            ClassID = Convert.ToInt32(learnerWorksheet.Cells[row, 9].Value),
                            Parents =
                            [
                                new()
                                {
                                    LearnerIdNo = learnerWorksheet.Cells[row, 5].Value?.ToString(),
                                    ParentIdNo = parents[learnerIdNos.IndexOf(Convert.ToInt64(learnerWorksheet.Cells[row, 5].Value))].IdNo,
                                    Parent = parents[learnerIdNos.IndexOf(Convert.ToInt64(learnerWorksheet.Cells[row, 5].Value))]
                                }
                            ],
                            Role = "Learner"
                        };

                        _returnDictionary = await LearnerExistsAsync(learner);
                        if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                        _returnDictionary = AttemptObjectValidation(learner);
                        if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                        var school = await _context.Schools.Include(s => s.SchoolGradeNP).FirstOrDefaultAsync(s => s.Id == learner.SchoolID);
                        var classes = await _context.SubGrade.ToListAsync();
                        if (school == null) throw new($"Learner {learner.Name} registration failed. Could not find a school with the ID {learner.SchoolID}.");

                        learner.LearnerSchoolNP = school;

                        _returnDictionary = AddClassDetailsToLearner(ref learner);
                        if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                        foreach (var parent in parents)
                        {
                            _returnDictionary = await _groupRepo.AddActorToGroup(parent.IdNo, learner.SchoolID, "All");
                            if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                        }
                        learners.Add(learner);
                    }
                }

                foreach (var parent in parents)
                {
                    _returnDictionary = await _signInRepo.CreateUserAccountAsync(parent.EmailAddress, parent.Role, parent.PhoneNumber.ToString());
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                }

                await _context.AddRangeAsync(learners);
                await _context.SaveChangesAsync();

                _returnDictionary["Success"] = true;
                _returnDictionary["Result"] = learners;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message + "\nInner Exception: " + ex.InnerException;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> CreateAsync(Learner learner)
        {
            try
            {
                var schools = await _context.Schools.Include(s => s.SchoolGradeNP)!.ThenInclude(g => g.Classes).ToListAsync();
                learner.LearnerSchoolNP = schools!.FirstOrDefault(s => s.Id == learner.SchoolID) 
                    ?? throw new($"Learner registration failed. Could not find a school with the ID {learner.SchoolID}.");

                _returnDictionary = AddClassDetailsToLearner(ref learner);
                if (!(bool)_returnDictionary["Success"])
                    throw new(_returnDictionary["ErrorMessage"] as string);

                List<Parent> parentsToAttach = [];
                List<string> returnErrors = [];

                _returnDictionary = await LearnerExistsAsync(learner);
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                var parents = await _context.Parents.ToListAsync();
                _returnDictionary = CheckAndAddParents(ref learner, ref parents, ref parentsToAttach);
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                foreach (var parent in learner.Parents)
                {
                    _returnDictionary = await _signInRepo.CreateUserAccountAsync(parent.Parent!.EmailAddress, parent.Parent.Role, parent.Parent.PhoneNumber.ToString());
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                }

                if (learner.ProfileImageFile is not null)
                {
                    _returnDictionary = SaveImage($"{learner.Name} {learner.Surname} - {learner.LearnerSchoolNP!.Name}", "Profile Images Folder/Learners", learner.ProfileImageFile);
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                    learner.ProfileImage = _returnDictionary["FileName"] as string;
                }
                else
                    learner.ProfileImage = "Default Pic.png";

                var lparent = learner.Parents.FirstOrDefault()?.Parent ?? throw new("Something went wrong, failed to acquire parent data along with the learner's data!!");
                if (lparent.ProfileImageFile is not null)
                {
                    _returnDictionary = SaveImage($"{lparent.Name} {lparent.Surname} - {learner.Name} {learner.LearnerSchoolNP!.Name}", "Profile Images Folder/Parents", lparent.ProfileImageFile);
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                    lparent.ProfileImage = _returnDictionary["FileName"] as string;
                }
                else
                    lparent.ProfileImage = "Default Pic.png";

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
                return await GetActorById(learnerId, new Learner(), _context);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = true;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetByIdNo(string learnerIdNo)
        {
            try
            {
                var learner = await _context.Learners.Include(s => s.LearnerSchoolNP).FirstOrDefaultAsync(l => l.IdNo == learnerIdNo);
                if (learner is null)
                    throw new("Could not find a learner with the specified ID number. Has the learner been registered yet?");

                _returnDictionary["Success"] = true;
                _returnDictionary["Result"] = learner;
                return _returnDictionary;
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
