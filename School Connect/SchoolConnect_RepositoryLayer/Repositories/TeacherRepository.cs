using SchoolConnect_RepositoryLayer.Interfaces;
using SchoolConnect_DomainLayer.Data;
using SchoolConnect_DomainLayer.Models;
using static SchoolConnect_RepositoryLayer.CommonAction.CommonActions;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml.Drawing.Chart.ChartEx;

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

                _returnDictionary = _groupRepo.AddTeacherToGroup(ref teacher, teacher.SchoolID, "All");
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
                    .AsNoTracking()
                    .Include(m => m.MainClass)
                    .Include(c => c.Classes)
                    .ThenInclude(c => c.Class)
                    .Include(t => t.TeacherSchoolNP)
                    .ThenInclude(a => a!.SchoolAnnouncementsNP)
                    .FirstOrDefaultAsync(t => t.Id == teacherId);
                if (teacher is null) throw new("Could not find a teacher with the specified ID.");

                teacher.AnnouncementsNP = null;
                teacher.TeacherSchoolNP!.SchoolGroupsNP = [.. _context.Groups.AsNoTracking().Where(g => g.SchoolID == teacher.SchoolID)];

                teacher.TeacherSchoolNP!.SchoolTeachersNP = null;

                teacher.TeacherSchoolNP.SchoolAnnouncementsNP.ToList().ForEach(ann => ann.AnnouncementSchoolNP = null);
                teacher.TeacherSchoolNP.SchoolGroupsNP.ToList().ForEach(group => group.GroupSchoolNP = null);
                teacher.Classes.ToList().ForEach(cls => 
                {
                    cls.Teacher = null;
                    cls.Class.Teachers = null;
                });

                _returnDictionary["Success"] = true;
                _returnDictionary["Result"] = teacher;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message + "\nInner Exception: " + ex.InnerException;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetAttendanceRecordsByTeacherAsync(long teacherId)
        {
            try
            {
                var teacher = await _context.Teachers
                    .AsNoTracking()
                    .Include(m => m.MainClass)
                    .ThenInclude(l => l.Learners)!
                    .ThenInclude(a => a.AttendanceRecords)
                    .Include(a => a.AttendanceRecords)
                    .FirstOrDefaultAsync(t => t.Id == teacherId);
                if (teacher is null) throw new("Could not find a teacher with the specified ID.");

                _returnDictionary["Success"] = true;
                _returnDictionary["Result"] = teacher;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message + "\nInner Exception: " + ex.InnerException;
                return _returnDictionary;
            }
        }

        public Task<Dictionary<string, object>> GetTeacherByClass(string classId)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, object>> Remove(long teacherId)
        {
            throw new NotImplementedException();
        }

        public async Task<Dictionary<string, object>> UpdatePersonalInfoAsync(Teacher teacher)
        {
            try
            {
                var existingTeacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == teacher.Id);
                if (existingTeacher is null) throw new("Could not find a teacher with the specified ID");

                if (teacher.EmailAddress != existingTeacher.EmailAddress)
                {
                    _returnDictionary = await _signInRepo.ChangeEmailAddressAsync(existingTeacher.EmailAddress, teacher.EmailAddress);
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                }

                if (teacher.PhoneNumber != existingTeacher.PhoneNumber)
                {
                    _returnDictionary = await _signInRepo.ChangePhoneNumberAsync(existingTeacher.PhoneNumber.ToString(), teacher.PhoneNumber.ToString(), teacher.EmailAddress);
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                }

                existingTeacher.EmailAddress = teacher.EmailAddress;
                existingTeacher.PhoneNumber = teacher.PhoneNumber;

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
                            ProfileImage = worksheet.Cells[row, 1].Value?.ToString()!.Trim(),
                            Title = worksheet.Cells[row, 2].Value?.ToString()!.Trim(),
                            Name = worksheet.Cells[row, 3].Value?.ToString()!.Trim(),
                            Surname = worksheet.Cells[row, 4].Value?.ToString()!.Trim(),
                            Gender = worksheet.Cells[row, 5].Value?.ToString()!.Trim(),
                            StaffNr = worksheet.Cells[row, 6].Value?.ToString()!.Trim()!,
                            Subjects = worksheet.Cells[row, 7].Value?.ToString()!.Trim()
                            .Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(subject => subject.Trim())
                            .Where(subject => !string.IsNullOrWhiteSpace(subject))
                            .ToList()!,
                            PhoneNumber = Convert.ToInt64(worksheet.Cells[row, 8].Value),
                            EmailAddress = worksheet.Cells[row, 9].Value?.ToString()!.Trim()!,
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

                        _returnDictionary = _groupRepo.AddTeacherToGroup(ref teacher, schoolId, "All");
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

        public async Task<Dictionary<string, object>> GetTeachersBySchoolAsync(long schoolId)
        {
            try
            {
                var school = await _context.Schools.AsNoTracking().Include(a => a.SchoolTeachersNP).FirstOrDefaultAsync(s => s.Id == schoolId);
                if (school is null) throw new("Could not find a school with the specified ID.");

                _returnDictionary["Success"] = true;
                _returnDictionary["Result"] = school.SchoolTeachersNP!;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message + "\nInner Exception: " + ex.InnerException;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetTeacherByEmailAddressAsync(string email)
        {
            try
            {
                var teacher = await _context.Teachers.AsNoTracking().FirstOrDefaultAsync(t => t.EmailAddress == email);
                if (teacher is null) throw new("Could not find a teacher with the specified email address.");

                _returnDictionary["Success"] = true;
                _returnDictionary["Result"] = teacher;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message + "\nInner Exception: " + ex.InnerException;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> UpdateClassAllocationAsync(Teacher teacher)
        {
            try
            {
                var existingTeacher = await _context.Teachers.Include(m => m.MainClass).Include(c => c.Classes).FirstOrDefaultAsync(t => t.Id == teacher.Id);
                if (existingTeacher is null) throw new("Could not find the teacher who's information is being updated.");
                
                if (teacher.MainClass is not null)
                {
                    if (existingTeacher.MainClass is not null)
                        throw new("This teacher already has a main class associated with them. A teacher can not have more than one main class.");

                    if (!teacher.Subjects.Intersect(teacher.MainClass.SubjectsTaught).IsNullOrEmpty())
                    {
                        existingTeacher.MainClass = teacher.MainClass;
                        _returnDictionary = _groupRepo.AddTeacherToGroup(ref existingTeacher, teacher.SchoolID, $"Grade {teacher.MainClass.ClassDesignate} Teachers");
                        if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                    }
                    else
                        throw new("Cannot assign the main teacher role for this class to this teacher. They do not teacher any of the subjects taught in this class.");
                }
                
                if (teacher.Classes is not null)
                {
                    if (existingTeacher.Classes is not null)
                        teacher.Classes = teacher.Classes.Where(cls => existingTeacher.Classes.FirstOrDefault(c => c.ClassDesignate == cls.ClassDesignate) == null).ToList();

                    if (!teacher.Classes.Any())
                        throw new("This teacher already teaches the provided class.");

                    var cls = await _context.SubGrade.AsNoTracking().FirstOrDefaultAsync(c => c.ClassDesignate == teacher.Classes.First().Class!.ClassDesignate);
                    if (cls is null) throw new($"Could not find the class {teacher.Classes.First().Class!.ClassDesignate} in the dictionary.");

                    if (!teacher.Subjects.Intersect(cls.SubjectsTaught).IsNullOrEmpty())
                    {
                        existingTeacher.Classes = teacher.Classes;
                        _returnDictionary = _groupRepo.AddTeacherToGroup(ref existingTeacher, teacher.SchoolID, $"Grade {teacher.Classes.First().ClassDesignate} Teachers");
                        if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                    }
                    else
                        throw new("The specified teacher does not teach any subjects taught in this grade.");
                }

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

        public async Task<Dictionary<string, object>> MarkClassAttendanceAsync(IEnumerable<Attendance> attendanceRecords)
        {
            try
            {
                if (!attendanceRecords.Any()) throw new("Something went wrong, empty attendance record provided.");

                var existingAttendanceRecords = await _context.Attendance.AsNoTracking().Where(a => a.ClassID == attendanceRecords.First().ClassID).ToListAsync();
                foreach (var existingAttRecs in existingAttendanceRecords)
                    attendanceRecords = attendanceRecords.Where(a => a.Date.Date != existingAttRecs.Date.Date);

                if (!attendanceRecords.Any()) throw new("Attendance records for the specified date already exist.");
                
                _context.AddRange(attendanceRecords);
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

        public async Task<Dictionary<string, object>> GetParentsByTeacherClassesAsync(long teacherId)
        {
            try
            {
                var teacher = await _context.Teachers
                    .AsNoTracking()
                    .Include(m => m.MainClass)
                    .ThenInclude(l => l.Learners)!
                    .ThenInclude(lp => lp.Parents)
                    .ThenInclude(p => p.Parent)
                    .Include(c => c.Classes)!
                    .ThenInclude(c => c.Class)
                    .ThenInclude(l => l.Learners)!
                    .ThenInclude(lp => lp.Parents)
                    .ThenInclude(p => p.Parent)
                    .FirstOrDefaultAsync(t => t.Id == teacherId);
                if (teacher is null) throw new("Could not find a teacher with the specified ID.");

                var chats = _context.Chats.AsNoTracking().Where(c => c.SenderIdentificate == teacher.StaffNr || c.ReceiverIdentificate == teacher.StaffNr).ToList();

                List<Parent> parents = [];
                if (teacher.MainClass is not null)
                {
                    foreach (var value in teacher.MainClass.Learners)
                        value.Parents.ToList().ForEach(val => parents.Add(val.Parent!));
                }

                if (!teacher.Classes.IsNullOrEmpty())
                {
                    foreach (var value in teacher.Classes)
                        value.Class!.Learners!.ToList().ForEach(val => val.Parents.ToList().ForEach(lp => parents.Add(lp.Parent!)));
                }

                if (parents.IsNullOrEmpty()) throw new("No parents associated with the specified Teacher's classes.");

                foreach (var parent in parents)
                {
                    _returnDictionary = await RetrieveImageAsBase64Async(parent.ProfileImage ?? "Default Pic.png", "Parents");
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                    parent.ProfileImageBase64 = _returnDictionary["Result"] as string;
                    parent.ProfileImageType = _returnDictionary["ImageType"] as string;

                    parent.Chats ??= [];
                    parent.Chats = [.. chats.Where(c => c.SenderIdentificate == parent.IdNo || c.ReceiverIdentificate == parent.IdNo)];

                    parent.Children = null;
                }

                _returnDictionary.Clear();
                _returnDictionary["Success"] = true;
                _returnDictionary["Result"] = parents;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message + "\nInner Exception: " + ex.InnerException;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetGradesByTeacherAsync(long teacherId)
        {
            try
            {
                var teacher = await _context.Teachers
                    .AsNoTracking()
                    .Include(t => t.TeacherSchoolNP)
                    .ThenInclude(s => s.SchoolGradesNP)!
                    .ThenInclude(g => g.Classes)
                    .ThenInclude(cls => cls.Teachers)
                    .FirstOrDefaultAsync(t => t.Id == teacherId);

                if (teacher is null) throw new("Could not find a teacher with the specified ID.");

                var grades = teacher.TeacherSchoolNP.SchoolGradesNP
                    .Where(grade => grade.Classes
                        .Any(cls => cls.Teachers.Any(tg => tg.TeacherID == teacherId)))
                    .ToList();

                grades.ForEach(grade =>
                {
                    grade.Classes = grade.Classes
                        .Where(cls => cls.Teachers.Any(tg => tg.TeacherID == teacherId))
                        .ToList();

                    grade.GradeSchoolNP = null;
                    grade.Classes.ToList().ForEach(cls => cls.Grade = null);
                });

                _returnDictionary["Success"] = true;
                _returnDictionary["Result"] = grades;
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
