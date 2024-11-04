using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using SchoolConnect_DomainLayer.Data;
using SchoolConnect_DomainLayer.Models;
using static SchoolConnect_RepositoryLayer.CommonAction.CommonActions;
using SchoolConnect_RepositoryLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

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
                foreach (var grade in learner.LearnerSchoolNP!.SchoolGradesNP!)
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
        public async Task<Dictionary<string, object>> BatchLoadLearnersFromExcelAsync(IFormFile learnerSpreadSheet, long schoolId)
        {
            try
            {
                _returnDictionary = SaveFile(learnerSpreadSheet.FileName, "Misc", learnerSpreadSheet);
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                var learners = new List<Learner>();
                var parents = new List<Parent>();
                var learnerIdNos = new List<long>();

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                string folderPath = @"C:\Users\innoc\Desktop\Git Repo\School-Connect\School Connect\SchoolConnect_DomainLayer\Application Files\Misc\";
                using (var stream = new FileStream(folderPath + learnerSpreadSheet.FileName, FileMode.Open, FileAccess.Read))
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
                            ProfileImage = parentWorksheet.Cells[row, 1].Value?.ToString()!.Trim(),
                            Name = parentWorksheet.Cells[row, 2].Value?.ToString()!.Trim(),
                            Surname = parentWorksheet.Cells[row, 3].Value?.ToString()!.Trim(),
                            Gender = parentWorksheet.Cells[row, 4].Value?.ToString()!.Trim(),
                            IdNo = parentWorksheet.Cells[row, 5].Value?.ToString()!.Trim()!,
                            ParentType = parentWorksheet.Cells[row, 6].Value?.ToString(),
                            EmailAddress = parentWorksheet.Cells[row, 7].Value?.ToString(),
                            PhoneNumber = Convert.ToInt64(parentWorksheet.Cells[row, 8].Value),
                            Role = "Parent",
                            Title = parentWorksheet.Cells[row, 9].Value?.ToString()
                        };

                        _returnDictionary = AttemptObjectValidation(parent);
                        if (!(bool)_returnDictionary["Success"])
                        {
                            var errors = _returnDictionary["Errors"] as List<string>;
                            string longErrorString = "";
                            errors!.ForEach(x => longErrorString += $"{x}\n");
                            throw new(longErrorString);
                        }

                        parents.Add(parent);
                        learnerIdNos.Add(Convert.ToInt64(parentWorksheet.Cells[row, 10].Value));
                    }

                    var schools = await _context.Schools.Include(g => g.SchoolGradesNP).ThenInclude(g => g.Classes).ToListAsync();
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
                            SchoolID = schoolId,
                            Parents =
                            [
                                new()
                                {
                                    LearnerIdNo = learnerWorksheet.Cells[row, 5].Value?.ToString(),
                                    ParentIdNo = parents[learnerIdNos.IndexOf(Convert.ToInt64(learnerWorksheet.Cells[row, 5].Value))].IdNo,
                                    Parent = parents[learnerIdNos.IndexOf(Convert.ToInt64(learnerWorksheet.Cells[row, 5].Value))]
                                }
                            ],
                            Role = "Learner",
                            Title = learnerWorksheet.Cells[row, 7].Value?.ToString(),
                        };

                        _returnDictionary = await LearnerExistsAsync(learner);
                        if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                        _returnDictionary = AttemptObjectValidation(learner);
                        if (!(bool)_returnDictionary["Success"])
                        {
                            var errors = _returnDictionary["Errors"] as List<string>;
                            string longErrorString = "";
                            errors!.ForEach(x => longErrorString += $"{x}\n");
                            throw new(longErrorString);
                        }

                        var school = await _context.Schools.Include(s => s.SchoolGradesNP)!.ThenInclude(c => c.Classes).FirstOrDefaultAsync(s => s.Id == learner.SchoolID);
                        if (school == null) throw new($"Learner {learner.Name} registration failed. Could not find a school with the ID {learner.SchoolID}.");

                        learner.LearnerSchoolNP = school;

                        _returnDictionary = AddClassDetailsToLearner(ref learner);
                        if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                        
                        learners.Add(learner);
                    }
                }

                foreach (var parent in parents)
                {
                    _returnDictionary = _groupRepo.AddParentToGroup(parent, schoolId, "All");
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                    _returnDictionary = await _signInRepo.CreateUserAccountAsync(parent.EmailAddress, parent.Role, parent.PhoneNumber.ToString());
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                }

                await _context.AddRangeAsync(learners);
                await _context.SaveChangesAsync();

                _returnDictionary = DeleteFile(@"C:\Users\innoc\Desktop\Git Repo\School-Connect\School Connect\SchoolConnect_DomainLayer\Application Files\Misc\" + learnerSpreadSheet.FileName);
                if (!(bool)_returnDictionary["Success"]) _returnDictionary["AdditionalInformation"] = _returnDictionary["ErrorMessage"];

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

        public async Task<Dictionary<string, object>> CreateAsync(Learner learner)
        {
            try
            {
                var schools = await _context.Schools.Include(s => s.SchoolGradesNP)!.ThenInclude(g => g.Classes).ToListAsync();
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
                    _returnDictionary = SaveFile($"{learner.Name} {learner.Surname} - {learner.LearnerSchoolNP!.Name}", "Profile Images Folder/Learners", learner.ProfileImageFile);
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                    learner.ProfileImage = _returnDictionary["FileName"] as string;
                }
                else
                    learner.ProfileImage = "Default Pic.png";

                var lparent = learner.Parents.FirstOrDefault()?.Parent ?? throw new("Something went wrong, failed to acquire parent data along with the learner's data!!");
                if (lparent.ProfileImageFile is not null)
                {
                    _returnDictionary = SaveFile($"{lparent.Name} {lparent.Surname} - {learner.Name} {learner.LearnerSchoolNP!.Name}", "Profile Images Folder/Parents", lparent.ProfileImageFile);
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

        public async Task<Dictionary<string, object>> GetLearnerByIdAsync(long learnerId)
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

        public async Task<Dictionary<string, object>> GetLearnerByIdNoAsync(string learnerIdNo)
        {
            try
            {
                var learner = await _context.Learners
                    .AsNoTracking()
                    .Include(s => s.LearnerSchoolNP)
                    .Include(c => c.Class)
                    .ThenInclude(m => m.MainTeacher)
                    .Include(p => p.Parents)
                    .ThenInclude(p => p.Parent)
                    .FirstOrDefaultAsync(l => l.IdNo == learnerIdNo);
                if (learner is null)
                    throw new("Could not find a learner with the specified ID number. Has the learner been registered yet?");

                learner.LearnerSchoolNP.SchoolLearnersNP = null;
                learner.Parents.ToList().ForEach(lp => lp.Parent.Children = null);
                learner.Class.Learners = null;

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

        public async Task<Dictionary<string, object>> GetLearnersByClassAsync(long teacherId)
        {
            try
            {
                var cls = await _context.SubGrade
                    .Include(a => a.Learners)!
                    .Include(m => m.MainTeacher)
                    .FirstOrDefaultAsync(c => c.MainTeacherId == teacherId);
                if (cls is null) throw new("Could not find a class whose main teacher is the specified teacher.");

                if (cls.Learners.IsNullOrEmpty()) throw new("The specified class does not yet have any learners assigned to it.");

                _returnDictionary["Success"] = true;
                _returnDictionary["Result"] = cls.Learners!;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message + "\nInnerException: " + ex.InnerException;
                return _returnDictionary;
            }
        }

        public Task<Dictionary<string, object>> Remove(long learnerId)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, object>> RemoveClass(string classId)
        {
            throw new NotImplementedException();
        }

        public async Task<Dictionary<string, object>> UpdateAsync(Learner learner)
        {
            try
            {
                var existingLearner = await _context.Learners.FirstOrDefaultAsync(l => l.Id == learner.Id);
                if (existingLearner is null) throw new("Could not find a learner with the specified ID.");

                _context.Update(learner);
                _context.SaveChanges();

                _returnDictionary["Success"] = true;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message + "\nInnerException: " + ex.InnerException;
                return _returnDictionary;
            }
        }
    }
}
