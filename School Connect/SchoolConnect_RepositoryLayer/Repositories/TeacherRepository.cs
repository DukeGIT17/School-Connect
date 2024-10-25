using SchoolConnect_RepositoryLayer.Interfaces;
using SchoolConnect_DomainLayer.Data;
using SchoolConnect_DomainLayer.Models;
using static SchoolConnect_RepositoryLayer.CommonAction.CommonActions;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace SchoolConnect_RepositoryLayer.Repositories
{
    public class TeacherRepository : ITeacher
    {
        private readonly SchoolConnectDbContext _context;
        private readonly ISignInRepo _signInRepo;
        private readonly IGroupRepo _groupRepo;
        private Dictionary<string, object> _returnDictionary;

        public TeacherRepository(SchoolConnectDbContext context, ISignInRepo signInRepo, IGroupRepo groupRepo)
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

                _returnDictionary = _groupRepo.AddTeacherToGroup(teacher, teacher.SchoolID, "All");
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                _returnDictionary = await _signInRepo.CreateUserAccountAsync(teacher.EmailAddress, teacher.Role, teacher.PhoneNumber.ToString());
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                if (teacher.ProfileImageFile is not null)
                {
                    _returnDictionary = SaveFile($"{teacher.Name} {teacher.Surname} - {teacher.TeacherSchoolNP!.Name}", "Profile Images Folder/Teachers", teacher.ProfileImageFile);
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                    teacher.ProfileImage = _returnDictionary["FileName"] as string;
                }
                else
                    teacher.ProfileImage = "Default Pic.jpeg";

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

        public async Task<Dictionary<string, object>> GetByIdAsync(long teacherId)
        {
            try
            {
                var teacher = await _context.Teachers
                    .Include(t => t.TeacherSchoolNP)
                    .ThenInclude(a => a!.SchoolAnnouncementsNP)
                    .FirstOrDefaultAsync();
                if (teacher is null) throw new("Could not find a teacher with the specified ID.");

                teacher.AnnouncementsNP = null;
                teacher.TeacherSchoolNP!.SchoolGroupsNP = [.. _context.Groups.Where(g => g.SchoolID == teacher.SchoolID)];

                teacher.TeacherSchoolNP!.SchoolTeachersNP = null;

                foreach (var ann in teacher.TeacherSchoolNP.SchoolAnnouncementsNP)
                {
                    if (ann.AnnouncementSchoolNP is not null)
                    {
                        ann.AnnouncementSchoolNP = null;
                    }
                }

                foreach (var group in teacher.TeacherSchoolNP.SchoolGroupsNP)
                {
                    if (group.GroupSchoolNP is not null)
                        group.GroupSchoolNP = null;
                }
                _returnDictionary["Success"] = true;
                _returnDictionary["Result"] = teacher;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
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

        public async Task<Dictionary<string, object>> UpdateAsync(Teacher teacher)
        {
            try
            {
                var existingTeacher = await _context.Teachers.AsNoTracking().FirstOrDefaultAsync(t => t.Id == teacher.Id);
                if (existingTeacher is null) throw new("Could not find a teacher with the specified ID");

                teacher.Subjects = existingTeacher.Subjects;

                if (teacher.EmailAddress != existingTeacher.EmailAddress)
                {
                    _returnDictionary = await _signInRepo.ChangeEmailAddressAsync(existingTeacher.EmailAddress, teacher.EmailAddress);
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                }

                if (teacher.PhoneNumber != existingTeacher.PhoneNumber)
                {
                    _returnDictionary = await _signInRepo.ChangePhoneNumberAsync(existingTeacher.PhoneNumber.ToString(), teacher.PhoneNumber.ToString(), existingTeacher.EmailAddress);
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                }

                _context.Update(teacher);
                _context.SaveChanges();

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

        public async Task<Dictionary<string, object>> BulkLoadTeacherFromExcel(IFormFile teacherSpreadsheet, long schoolId)
        {
            try
            {
                var school = await _context.Schools.FirstOrDefaultAsync(s => s.Id == schoolId);
                if (school is null) throw new("Something went wrong! Could not find a school with the specified ID.");

                _returnDictionary = SaveFile(teacherSpreadsheet.FileName, "Misc", teacherSpreadsheet);
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                List<Teacher> teachers = [];

                string folderPath = @"C:\Users\innoc\Desktop\Git Repo\School-Connect\School Connect\SchoolConnect_DomainLayer\Application Files\Misc\";
                using (var stream = new FileStream(folderPath + teacherSpreadsheet.FileName, FileMode.Open, FileAccess.Read))
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var teacher = new Teacher
                        {
                            ProfileImage = worksheet.Cells[row, 1].Value?.ToString(),
                            Title = worksheet.Cells[row, 2].Value?.ToString(),
                            Name = worksheet.Cells[row, 3].Value?.ToString(),
                            Surname = worksheet.Cells[row, 4].Value?.ToString(),
                            Gender = worksheet.Cells[row, 5].Value?.ToString(),
                            StaffNr = worksheet.Cells[row, 6].Value?.ToString()!,
                            Subjects = worksheet.Cells[row, 7].Value?.ToString()
                            .Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(subject => subject.Trim())
                            .Where(subject => !string.IsNullOrWhiteSpace(subject))
                            .ToList()!,
                            PhoneNumber = Convert.ToInt64(worksheet.Cells[row, 8].Value),
                            EmailAddress = worksheet.Cells[row, 9].Value?.ToString()!,
                            SchoolID = schoolId,
                            Role = "Teacher"
                        };

                        _returnDictionary = AttemptObjectValidation(teacher);
                        if (!(bool)_returnDictionary["Success"])
                        {
                            var errors = _returnDictionary["Errors"] as List<string>;
                            string longErrorString = "";
                            errors!.ForEach(x => longErrorString += $"{x}\n");
                            throw new(longErrorString);
                        }

                        _returnDictionary = _groupRepo.AddTeacherToGroup(teacher, schoolId, "All");
                        if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                        _returnDictionary = await _signInRepo.CreateUserAccountAsync(teacher.EmailAddress, teacher.Role, teacher.PhoneNumber.ToString());
                        if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                        teacher.TeacherSchoolNP = school;

                        teachers.Add(teacher);
                    }
                }

                _returnDictionary = DeleteFile(@"C:\Users\innoc\Desktop\Git Repo\School-Connect\School Connect\SchoolConnect_DomainLayer\Application Files\Misc\" + teacherSpreadsheet.FileName);
                if (!(bool)_returnDictionary["Success"]) _returnDictionary["AdditionalInformation"] = _returnDictionary["ErrorMessage"];

                await _context.AddRangeAsync(teachers);
                await _context.SaveChangesAsync();

                _returnDictionary["Success"] = true;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary = DeleteFile(@"C:\Users\innoc\Desktop\Git Repo\School-Connect\School Connect\SchoolConnect_DomainLayer\Application Files\Misc\" + teacherSpreadsheet.FileName);
                if (!(bool)_returnDictionary["Success"]) _returnDictionary["AdditionalInformation"] = _returnDictionary["ErrorMessage"];
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message + "\nInner Exception: " + ex.InnerException;
                return _returnDictionary;
            }
        }
    }
}
