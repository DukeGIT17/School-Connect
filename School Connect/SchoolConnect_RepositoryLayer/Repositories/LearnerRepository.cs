using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
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

        private Dictionary<string, object> AddClasses(List<School> schools, List<SubGrade> classes)
        {
            try
            {
                foreach (var school in schools)
                {
                    foreach (var grade in school.SchoolGradeNP!)
                    {
                        var classesToAdd = classes.Where(c => c.ClassDesignate.StartsWith(grade.GradeDesignate) && c.GradeId == grade.Id);

                        foreach (var cls in classesToAdd)
                        {
                            grade.Classes.Add(cls);
                        }
                    }
                }

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

        public async Task<Dictionary<string, object>> BatchLoadLearnersFromExcel(string fileName)
        {
            try
            {
                var learners = new List<Learner>();
                var parents = new List<Parent>();
                var learnerIdNos = new List<long>();

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                string folderPath = @"C:\Users\Lukhanyo\Documents\WIL\Project Excel Data Sheets\";
                using (var stream = new FileStream(folderPath + fileName, FileMode.Open, FileAccess.Read))
                using (var package = new ExcelPackage(stream))
                {
                    var learnerWorksheet = package.Workbook.Worksheets[0];
                    var parentWorksheet = package.Workbook.Worksheets[1];
                    var rowCount = learnerWorksheet.Dimension.Rows;
                    var parentRowCount = parentWorksheet.Dimension.Rows;

                    for (int row = 2; row < parentRowCount; row++)
                    {
                        var parent = new Parent
                        {
                            ProfileImage = parentWorksheet.Cells[row, 1].Value?.ToString(),
                            Name = parentWorksheet.Cells[row, 2].Value?.ToString(),
                            Surname = parentWorksheet.Cells[row, 3].Value?.ToString(),
                            Gender = parentWorksheet.Cells[row, 4].Value?.ToString(),
                            IdNo = Convert.ToInt64(parentWorksheet.Cells[row, 5].Value),
                            ParentType = parentWorksheet.Cells[row, 6].Value?.ToString(),
                            EmailAddress = parentWorksheet.Cells[row, 7].Value?.ToString(),
                            PhoneNumber = Convert.ToInt64(parentWorksheet.Cells[row, 8].Value),
                            Role = "Parent"
                        };

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
                            IdNo = Convert.ToInt64(learnerWorksheet.Cells[row, 5].Value),
                            ClassCode = learnerWorksheet.Cells[row, 6].Value?.ToString(),
                            Subjects = learnerWorksheet.Cells[row, 7].Value?.ToString()
                            .Trim('[', ']')
                            .Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(item => item.Trim(' ', '"', '\''))
                            .ToList(),
                            SchoolID = Convert.ToInt64(learnerWorksheet.Cells[row, 4].Value),
                            ClassID = Convert.ToInt32(learnerWorksheet.Cells[row, 4].Value),
                            Parents =
                            [
                                new()
                                {
                                    LearnerIdNo = Convert.ToInt64(learnerWorksheet.Cells[row, 5].Value),
                                    ParentIdNo = parents[learnerIdNos.IndexOf(Convert.ToInt64(learnerWorksheet.Cells[row, 5].Value))].IdNo,
                                    Parent = parents[learnerIdNos.IndexOf(Convert.ToInt64(learnerWorksheet.Cells[row, 5].Value))]
                                }
                            ],
                            Role = "Learner"
                        };

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
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }


        public async Task<Dictionary<string, object>> CreateAsync(Learner learner)
        {
            try
            {
                var schools = await _context.Schools.Include(s => s.SchoolGradeNP).ToListAsync();
                var classes = await _context.SubGrade.ToListAsync();

                _returnDictionary = AddClasses(schools, classes);
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                schools = _returnDictionary["Result"] as List<School>;

                learner.LearnerSchoolNP = schools!.FirstOrDefault(s => s.Id == learner.SchoolID)
                    ?? throw new($"Learner registration failed. Could not find a school with the ID {learner.SchoolID}.");

                bool found = false;
                foreach (var grade in learner.LearnerSchoolNP.SchoolGradeNP!)
                {
                    var cls = grade.Classes.FirstOrDefault(c => c.ClassDesignate == learner.ClassCode);
                    if (cls != null)
                    {
                        learner.Class = cls;
                        learner.ClassID = cls.Id;
                        found = true;
                        break;
                    }
                }

                if (!found)
                    throw new($"Could not find the class {learner.ClassCode} in this school.");

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
