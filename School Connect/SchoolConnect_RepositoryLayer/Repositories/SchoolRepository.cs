using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
                        GroupActorNP = []
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
                                        ClassDesignate = grade.ToString() + "A"
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
                                        ClassDesignate = grade.ToString() + "A"
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
                                        ClassDesignate = grade.ToString() + "A"
                                    }
                                ]
                            };
                            newSchool.SchoolGradesNP.Add(gr);
                        }
                        break;
                    default:
                        throw new($"Something went wrong. The value {newSchool.Type} does not match any of the valid school type options.");
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
                _returnDictionary = await _sysAdminRepo.GetAdminByIdAsync(id);
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                var admin = _returnDictionary["Result"] as SysAdmin;
                if (admin!.SysAdminSchoolNP is null)
                    throw new("Could not find a school linked to this admin's ID. Has this admin registered a school yet?");

                _returnDictionary.Clear();
                _returnDictionary["Success"] = true;
                _returnDictionary["Result"] = admin.SysAdminSchoolNP;
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
                var school = await _context.Schools.Include(s => s.SchoolAddress).FirstOrDefaultAsync(s => s.Id == schoolId);
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

        public Task<Dictionary<string, object>> UpdateSchoolInfo(School school)
        {
            throw new NotImplementedException();
        }
    }
}
