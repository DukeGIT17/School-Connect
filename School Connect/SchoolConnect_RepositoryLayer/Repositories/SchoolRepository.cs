using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SchoolConnect_DomainLayer.Data;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_RepositoryLayer.Interfaces;
using static SchoolConnect_RepositoryLayer.CommonAction.CommonActions;

namespace SchoolConnect_RepositoryLayer.Repositories
{
    public class SchoolRepository : ISchool
    {
        private readonly SchoolConnectDbContext _context;
        private readonly ISysAdmin _sysAdminRepo;
        private Dictionary<string, object> _returnDictionary;

        public SchoolRepository(SchoolConnectDbContext context, ISysAdmin sysAdminRepo)
        {
            _context = context;
            _sysAdminRepo = sysAdminRepo;
            _returnDictionary = [];
        }

        private int GetGradeFromClassDesignate(string classDesignate)
        {
            int rc = -1;
            if (classDesignate.First() == 'R')
                rc = 0;
            else
                rc = Convert.ToInt32(classDesignate.Remove(classDesignate.IndexOf(classDesignate.Last())));
            return rc;
        }

        private List<string> DetermineClassSubjects(int grade)
        {
            if (grade == 0)
                return ["Language and Literacy", "Mathematics", "Life Skills", "Physical Education"];
            else if (grade >= 1 && grade <= 3)
                return ["Home Language", "First Additional Language", "Mathematics", "Life Skills"];
            else if (grade >= 4 && grade <= 6)
                return ["Home Language", "First Additional Language", "Mathematics", "Natural Science and Technology", "Social Science Geography", "Social Science History", "Life Skills"];
            else if (grade >= 7 && grade <= 9)
                return ["Home Language", "First Additional Language", "Mathematics", "Natural Sciences", "Social Science Geography", "Social Science History", "Technology", "Economic And Management Science", "Life Orientation", "Creative Arts"];
            else if (grade >= 10 && grade <= 12)
                return ["Home Language", "First Additional Language", "Life Orientation"];
            else
                throw new("Something went wrong. Invalid grade provided");
        }

        public async Task<Dictionary<string, object>> RegisterSchoolAsync(School newSchool)
        {
            try
            {
                var schools = await _context.Schools.ToListAsync();
                var school = schools.FirstOrDefault(x => x.SystemAdminId == newSchool.SystemAdminId);
                if (school != null)
                {
                    var admin = await _context.SystemAdmins.FirstOrDefaultAsync(a => a.Id == newSchool.SystemAdminId);
                    if (admin == null)
                        throw new($"An admin can only register a single school, admin with the ID {school.SystemAdminId} already has a school associated with them in the system.");

                    throw new($"An admin can only register a single school, admin {admin.Name} {admin.Surname} with the ID {newSchool.SystemAdminId} already has a school associated with them in the system.");
                }

                school = schools.FirstOrDefault(x => x.EmisNumber == newSchool.EmisNumber);
                if (school != null)
                    throw new($"A school possessing the emis number, '{newSchool.EmisNumber}', already exists within the database.");

                newSchool.SchoolGroupsNP =
                [
                    new() {
                        GroupName = "All",
                        GroupMemberIDs = [],
                        GroupParentNP = []
                    }
                ];

                string primaryGrades = "R1234567";
                string[] highGrades = ["8", "9", "10", "11", "12"],
                    combinedGrades = ["R", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"];

                newSchool.SchoolGradesNP = [];

                switch (newSchool.Type)
                {
                    case "Primary":
                        foreach (var grade in primaryGrades)
                        {
                            Grade gr = new()
                            {
                                GradeDesignate = grade.ToString(),
                                Classes = 
                                [
                                    new() 
                                    {
                                        ClassDesignate = grade.ToString() + "A",
                                        SubjectsTaught = grade == 'R' ? DetermineClassSubjects(0) : DetermineClassSubjects(int.Parse(grade.ToString())),
                                    }
                                ]
                            };
                            newSchool.SchoolGradesNP.Add(gr);
                        }
                        break;

                    case "High":
                        foreach (var grade in highGrades)
                        {
                            Grade gr = new()
                            {
                                GradeDesignate = grade.ToString(),
                                Classes =
                                [
                                    new()
                                    {
                                        ClassDesignate = grade.ToString() + "A",
                                        SubjectsTaught = DetermineClassSubjects(int.Parse(grade.ToString())),
                                    }
                                ]
                            };
                            newSchool.SchoolGradesNP.Add(gr);
                        }
                        break;
                    case "Combined":
                        foreach (var grade in combinedGrades)
                        {
                            Grade gr = new()
                            {
                                GradeDesignate = grade.ToString(),
                                Classes =
                                [
                                    new()
                                    {
                                        ClassDesignate = grade.ToString() + "A",
                                        SubjectsTaught = grade == "R" ? DetermineClassSubjects(0) : DetermineClassSubjects(int.Parse(grade.ToString())),

                                    }
                                ]
                            };
                            newSchool.SchoolGradesNP.Add(gr);
                        }
                        break;
                    default:
                        throw new($"Something went wrong. The value {newSchool.Type} does not match any of the valid school type options.");
                }

                foreach (var grade in newSchool.SchoolGradesNP)
                {
                    newSchool.SchoolGroupsNP.Add(new()
                    {
                        GroupMemberIDs = [],
                        GroupName = $"Grade {grade.GradeDesignate}A Parents"
                    });

                    newSchool.SchoolGroupsNP.Add(new()
                    {
                        GroupMemberIDs = [],
                        GroupName = $"Grade {grade.GradeDesignate}A Teachers"
                    });
                }

                if (newSchool.SchoolLogoFile is not null)
                {
                    _returnDictionary = SaveFile($"{newSchool.Name}", "School Logos Folder", newSchool.SchoolLogoFile);
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                    newSchool.Logo = _returnDictionary["FileName"] as string;
                }
                else
                    newSchool.Logo = "Default Pic.png";

                await _context.AddAsync(newSchool);
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

        public async Task<Dictionary<string, object>> GetSchoolsAsync()
        {
            try
            {
                var schools = await _context.Schools.ToListAsync();
                if (schools.IsNullOrEmpty()) throw new Exception("No schools in the database.");
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

        public async Task<Dictionary<string, object>> GetSchoolByAdminAsync(long id)
        {
            try
            {
                var school = await _context.Schools.Include(a => a.SchoolPrincipalNP).FirstOrDefaultAsync(s => s.SystemAdminId == id);
                if (school is null) throw new("Could not find a school associated with the specified administrator.");

                _returnDictionary.Clear();
                _returnDictionary["Success"] = true;
                _returnDictionary["Result"] = school;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message + "\nInner Exception: " + ex.InnerException;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetSchoolByLearnerIdNoAsync(string learnerIdNo)
        {
            try
            {
                var learner = await _context.Learners.Include(s => s.LearnerSchoolNP).FirstOrDefaultAsync(l => l.IdNo == learnerIdNo);
                if (learner is null)
                    throw new("Could not a learner with the specified Identity number, has the learner been registered yet?");

                _returnDictionary["Success"] = true;
                _returnDictionary["Result"] = learner.LearnerSchoolNP!;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message + "\nInner Exception: " + ex.InnerException;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetSchoolByIdAsync(long schoolId)
        {
            try
            {
                var school = await _context.Schools
                    .AsNoTracking()
                    .Include(s => s.SchoolAddress)
                    .Include(p => p.SchoolPrincipalNP)
                    .Include(s => s.SchoolSysAdminNP)
                    .FirstOrDefaultAsync(s => s.Id == schoolId);
                if (school is null) throw new($"Could not find a school with the ID: {schoolId}");

                _returnDictionary["Success"] = true;
                _returnDictionary["Result"] = school;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message + "\nInner Exception: " + ex.InnerException;
                return _returnDictionary;
            }
        }

        public Task<Dictionary<string, object>> GetSchoolByName(string schoolName)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, object>> RemoveSchool(string emisNumber, long schoolId = -1)
        {
            throw new NotImplementedException();
        }

        public async Task<Dictionary<string, object>> UpdateSchoolInfoAsync(School school)
        {
            try
            {
                var existingSchool = await _context.Schools.AsNoTracking().FirstOrDefaultAsync(s => s.Id == school.Id);
                if (existingSchool is null) throw new("Could not find a school with the specified ID.");

                _context.Update(school);
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

        public async Task<Dictionary<string, object>> GetSchoolGradesAsync(long schoolId, string? fromGrade = null, string? toGrade = null)
        {
            try
            {
                var school = await _context.Schools
                    .AsNoTracking()
                    .Include(a => a.SchoolGradesNP)!
                    .ThenInclude(a => a.Classes)
                    .FirstOrDefaultAsync(s => s.Id == schoolId);
                if (school is null) throw new("Could not acquire a school with the specified ID.");

                List<Grade> grades = [];
                foreach (var grade in school.SchoolGradesNP!)
                    grades.Add(grade);

                if (fromGrade is not null || toGrade is not null)
                {
                    if (school.Type == "Primary")
                    {
                        if (fromGrade != "R")
                            if (Convert.ToInt32(fromGrade) > 7) throw new("The fromGrade filter value cannot be greater than 7 for a primary school.");
                    }

                    if (school.Type == "High")
                            if (Convert.ToInt32(fromGrade) < 8) throw new("The fromGrade filter value cannot be less than 8 for a high school.");

                    if (fromGrade is not null)
                        grades = grades.Where(grade => Convert.ToInt32(grade.GradeDesignate) >= Convert.ToInt32(fromGrade)).ToList();

                    if (toGrade is not null)
                        grades = grades.Where(grade => Convert.ToInt32(grade.GradeDesignate) <= Convert.ToInt32(toGrade)).ToList();
                }

                foreach (var grade in grades)
                {
                    if (grade.GradeSchoolNP!.SchoolGradesNP is not null)
                        grade.GradeSchoolNP.SchoolGradesNP = null;
                }

                _returnDictionary["Success"] = true;
                _returnDictionary["Result"] = grades;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message + "\nInnerException: " + ex.InnerException;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetAllClassesBySchoolAsync(long schoolId)
        {
            try
            {
                var classes = await _context.SubGrade
                    .AsNoTracking()
                    .Include(g => g.Grade)!
                    .ToListAsync();
                if (classes is null) throw new("Could not find any classes in the database.");

                classes = classes.Where(a => a.Grade!.SchoolID == schoolId).ToList();
                classes.ForEach(cls => cls.Grade = null);

                _returnDictionary["Success"] = true;
                _returnDictionary["Result"] = classes;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message + "\nInner Exception: " + ex.InnerException;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetClassBySchoolAsync(string classDesignate, long schoolId)
        {
            try
            {
                var classes = await _context.SubGrade.AsNoTracking().Include(g => g.Grade).ToListAsync();
                if (classes is null) throw new("Could not find any classes in the database.");

                var clss = classes.FirstOrDefault(cls => cls.ClassDesignate == classDesignate && cls.Grade!.SchoolID == schoolId);
                if (clss is null) throw new("Could not find the specified class in the specified school.");

                clss.Grade = null;

                _returnDictionary["Success"] = true;
                _returnDictionary["Result"] = clss;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message + "\nInner Exception: " + ex.InnerException;
                return _returnDictionary;
            }
            }

        public async Task<Dictionary<string, object>> AddClassesToSchoolAsync(List<string> classDesignates, long schoolId)
        {
            try
            {
                var school = await _context.Schools.Include(sg => sg.SchoolGroupsNP).Include(g => g.SchoolGradesNP)!.ThenInclude(c => c.Classes).FirstOrDefaultAsync(s => s.Id == schoolId);
                if (school is null) throw new("Could not find a school with the specified ID.");

                if (school.Type == "Primary")
                {
                    classDesignates = classDesignates.Where(designate => GetGradeFromClassDesignate(designate) > -1 && GetGradeFromClassDesignate(designate) < 8).ToList();
                    if (classDesignates.IsNullOrEmpty()) throw new("The provided classes are not appropriate for the school type Primary");
                }
                else if (school.Type == "High")
                {
                    classDesignates = classDesignates.Where(designate => GetGradeFromClassDesignate(designate) > 7 && GetGradeFromClassDesignate(designate) < 13).ToList();
                    if (classDesignates.IsNullOrEmpty()) throw new("The provided classes are not appropriate for the school type High");
                }
                else
                {
                    classDesignates = classDesignates.Where(designate => GetGradeFromClassDesignate(designate) > -1 && GetGradeFromClassDesignate(designate) < 13).ToList();
                    if (classDesignates.IsNullOrEmpty()) throw new("The provided classes are not appropriate for the school type Combined");
                }

                foreach (var grade in school.SchoolGradesNP!)
                    classDesignates = classDesignates.Where(designate => grade.Classes.FirstOrDefault(c => c.ClassDesignate == designate) == null).ToList();

                if (!classDesignates.IsNullOrEmpty())
                {
                    foreach (var grade in school.SchoolGradesNP)
                    {
                        var designate = classDesignates.FirstOrDefault(designate => grade.GradeDesignate == designate.Remove(designate.IndexOf(designate.Last())));
                        if (designate is not null)
                        {
                            grade.Classes.Add(new()
                            {
                                ClassDesignate = designate,
                                GradeId = grade.Id,
                                SubjectsTaught = grade.GradeDesignate == "R" ? DetermineClassSubjects(0) : DetermineClassSubjects(Convert.ToInt32(grade.GradeDesignate))
                            });

                            school.SchoolGroupsNP.Add(new()
                            {
                                GroupMemberIDs = [],
                                GroupName = $"Grade {designate} Teachers",
                                SchoolID = school.Id,
                            });
                        }
                    }

                    _context.SaveChanges();
                }
                else
                    throw new("All the provided classes already exist.");

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

        public async Task<Dictionary<string, object>> AddSubjectsToSchoolAsync(List<string> subjects, long schoolId, string? newClasses = null)
        {
            try
            {
                if (newClasses is null) throw new("Failed to acquire new classes.");

                var school = await _context.Schools.Include(c => c.SchoolGradesNP)!.ThenInclude(g => g.Classes).FirstOrDefaultAsync(s => s.Id == schoolId);
                if (school is null) throw new("Could not find a school with the specified ID.");

                List<SubGrade> classes = [];
                List<string> newClsDesignates = [.. newClasses.Split([',', ' '], StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim())];
                foreach (var grade in school.SchoolGradesNP!)
                    classes.AddRange(grade.Classes.Where(cls => newClsDesignates.Contains(cls.ClassDesignate)).ToList());

                classes = classes.Intersect(classes.Where(cls => GetGradeFromClassDesignate(cls.ClassDesignate) >= 10).ToList()).ToList();
                if (!classes.IsNullOrEmpty())
                {
                    foreach (var cls in classes)
                    {
                        foreach (var subject in subjects)
                            cls.SubjectsTaught.Add(subject);
                        cls.SubjectsTaught = cls.SubjectsTaught.Distinct().ToList();
                    }

                }
                else
                    throw new($"{school.Name} {school.Type} School does not have the provided classes. Or the classes present do not possess classes above grade 10 ({newClasses})");

                _context.SaveChanges();

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

        public async Task<Dictionary<string, object>> GetGradesBySchoolAsync(long schoolId)
        {
            try
            {
                var school = await _context.Schools.AsNoTracking().Include(g => g.SchoolGradesNP)!.ThenInclude(c => c.Classes).ThenInclude(s => s.MainTeacher).FirstOrDefaultAsync(s => s.Id == schoolId);
                if (school is null) throw new("Could not find a school with the specified ID.");

                _returnDictionary["Success"] = true;
                _returnDictionary["Result"] = school.SchoolGradesNP!;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message + "\nInner Exception: " + ex.InnerException;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetClassByMainTeacher(long teacherId)
        {
            try
            {
                var cls = await _context.SubGrade.Include(m => m.MainTeacher).FirstOrDefaultAsync(c => c.MainTeacherId == teacherId);
                if (cls is null) throw new("Could not find a class with the specified teacher as the main teacher.");

                 _returnDictionary["Success"] = true;
                _returnDictionary["Result"] = cls;
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
