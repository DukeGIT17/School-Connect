using SchoolConnect_DomainLayer.Data;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_RepositoryLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using static SchoolConnect_RepositoryLayer.CommonAction.CommonActions;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using Microsoft.IdentityModel.Tokens;

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

        public async Task<Dictionary<string, object>> BatchLoadParentsFromExcelAsync(IFormFile parentSpreadsheet)
        {
            try
            {
                var learners = await _context.Learners.ToListAsync();
                if (learners is null) throw new("Cannot register a parent while there are no learners in the database. Please register learners first.");

                _returnDictionary = SaveFile(parentSpreadsheet.FileName, "Misc", parentSpreadsheet);
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                List<Parent> parents = [];
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                string folderPath = @"C:\Users\innoc\Desktop\Git Repo\School-Connect\School Connect\SchoolConnect_DomainLayer\Application Files\Misc\";
                using (var stream = new FileStream(folderPath + parentSpreadsheet.FileName, FileMode.Open, FileAccess.Read))
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var parent = new Parent
                        {
                            ProfileImage = worksheet.Cells[row, 1].Value.ToString()!.Trim(),
                            Name = worksheet.Cells[row, 2].Value.ToString()!.Trim(),
                            Surname = worksheet.Cells[row, 3].Value.ToString()!.Trim(),
                            Gender = worksheet.Cells[row, 4].Value.ToString()!.Trim(),
                            IdNo = worksheet.Cells[row, 5].Value.ToString()!.Trim(),
                            ParentType = worksheet.Cells[row, 6].Value.ToString()!.Trim(),
                            EmailAddress = worksheet.Cells[row, 7].Value.ToString()!.Trim(),
                            PhoneNumber = Convert.ToInt64(worksheet.Cells[row, 8].Value),
                            Role = "Parent",
                            Title = worksheet.Cells[row, 9].Value.ToString()!.Trim(),
                            Children =
                            [
                                new()
                                {
                                    LearnerID = learners.FirstOrDefault(l => l.IdNo == worksheet.Cells[row, 10].Value.ToString()!.Trim())!.Id,
                                    LearnerIdNo = worksheet.Cells[row, 10].Value.ToString()!,
                                    Learner = learners.FirstOrDefault(l => l.IdNo == worksheet.Cells[row, 10].Value.ToString()!.Trim()),
                                    ParentIdNo = worksheet.Cells[row, 5].Value.ToString()!.Trim()
                                }
                            ]
                        };

                        _returnDictionary = AttemptObjectValidation(parent);
                        if (!(bool)_returnDictionary["Success"])
                        {
                            var errors = _returnDictionary["Errors"] as List<string>;
                            string longErrorString = "";
                            errors!.ForEach(x => longErrorString += $"{x}\n");
                            throw new(longErrorString);
                        }


                        _returnDictionary = _groupRepo.AddParentToGroup(parent, parent.Children.First().Learner!.SchoolID, "All");
                        if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                        parents.Add(parent);
                    }

                    foreach (var parent in parents)
                    {
                        _returnDictionary = await _signInRepo.CreateUserAccountAsync(parent.EmailAddress, parent.Role, parent.PhoneNumber.ToString());
                        if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                    }
                }

                _returnDictionary = DeleteFile(@"C:\Users\innoc\Desktop\Git Repo\School-Connect\School Connect\SchoolConnect_DomainLayer\Application Files\Misc\" + parentSpreadsheet.FileName);
                if (!(bool)_returnDictionary["Success"]) _returnDictionary["AdditionalInformation"] = _returnDictionary["ErrorMessage"];

                await _context.AddRangeAsync(parents);
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
                {
                    throw new("All the learners provided do not exist within the database thus this new parent has no children to be linked to. " +
                        "Please provide valid ID numbers or create profiles for the learners first.");
                }

                foreach (var learner in learnersToAttach)
                {
                    var lp = parent.Children.FirstOrDefault(l => l.LearnerIdNo == learner.IdNo);
                    if (lp != null)
                        lp.Learner = learner;
                }

                foreach (var learner in parent.Children)
                {
                    _returnDictionary = _groupRepo.AddParentToGroup(parent, learner.Learner!.SchoolID, "All");
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                }

                _returnDictionary = await _signInRepo.CreateUserAccountAsync(parent.EmailAddress, parent.Role, parent.PhoneNumber.ToString());
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                if (!errors.IsNullOrEmpty())
                    _returnDictionary["AdditionalInformation"] = errors;

                if (parent.ProfileImageFile is not null)
                {
                    _returnDictionary = SaveFile($"{parent.Name} {parent.Surname}", "Profile Images Folder/Parents", parent.ProfileImageFile);
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
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message + "\nInner Exception: " + ex.InnerException;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetParentByIdAsync(long parentId)
        {
            try
            {
                var parent = await _context.Parents
                    .Include(p => p.Children)!
                    .ThenInclude(p => p.Learner)
                    .ThenInclude(s => s.LearnerSchoolNP)
                    .FirstOrDefaultAsync(p => p.Id == parentId);
                if (parent == null) throw new("Could not find a parent with the specified ID.");

                parent.Children!.ToList().ForEach(lp =>
                {
                    lp.Learner!.Parents = [];
                    lp.Parent = null;
                    lp.Learner.LearnerSchoolNP!.SchoolLearnersNP = null;
                });

                _returnDictionary["Success"] = true;
                _returnDictionary["Result"] = parent;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message + "\nInner Exception: " + ex.InnerException;
                return _returnDictionary;
            }
        }

        public Task<Dictionary<string, object>> GetByIdNoAsync(string parentIdNo)
        {
            throw new NotImplementedException();
        }

        public async Task<Dictionary<string, object>> UpdateAsync(Parent parent)
        {
            try
            {
                var existingParent = await _context.Parents.FirstOrDefaultAsync(p => p.Id == parent.Id);
                if (existingParent is null) throw new("Could not find a parent with the specified ID.");

                if (parent.EmailAddress != existingParent.EmailAddress)
                {
                    _returnDictionary = await _signInRepo.ChangeEmailAddressAsync(existingParent.EmailAddress, parent.EmailAddress);
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                    existingParent.EmailAddress = parent.EmailAddress;
                }

                if (parent.PhoneNumber != existingParent.PhoneNumber)
                {
                    _returnDictionary = await _signInRepo.ChangePhoneNumberAsync(existingParent.PhoneNumber.ToString(), parent.PhoneNumber.ToString(), parent.EmailAddress);
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                    existingParent.PhoneNumber = parent.PhoneNumber;
                }

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
    }
}
