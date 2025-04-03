using Microsoft.EntityFrameworkCore;
using SchoolConnect_DomainLayer.Data;
using SchoolConnect_DomainLayer.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace SchoolConnect_RepositoryLayer.CommonAction
{
    public static class CommonActions
    {
        public const string ApplicationFilesPath = $@"C:\Users\tsoai\Desktop\Git Repo\School-Connect\School Connect\SchoolConnect_DomainLayer\Application Files";
        public static Dictionary<string, object> AttemptObjectValidation<T>(T obj)
        {
            Dictionary<string, object> _returnDictionary = [];
            try
            {
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(obj!, serviceProvider: null, items: null);
                bool isValid = Validator.TryValidateObject(obj!, validationContext, validationResults);
                List<string>? errors = [];

                if (!isValid)
                {
                    foreach (var validationResult in validationResults)
                        errors.Add(validationResult.ErrorMessage
                            ?? "Something went wrong: Check Common Actions Class in the Repository.");

                    _returnDictionary["Success"] = false;
                    _returnDictionary["Errors"] = errors;
                    return _returnDictionary;
                }

                _returnDictionary["Success"] = true;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["Errors"] = ex.Message;
                return _returnDictionary;
            }
        }

        public static async Task<Dictionary<string, object>> GetActorById<T>(long actorId, T actor, SchoolConnectDbContext context) where T : BaseActor
        {
            Dictionary<string, object> _returnDictionary = [];
            try
            {
                SysAdmin? admin = null;
                Principal? principal = null;
                Teacher? teacher = null;
                Parent? parent = null;
                Learner? learner = null;

                switch (actor!.GetType().Name)
                {
                    case "SysAdmin":
                        admin = await context.SystemAdmins.Include(s => s.SysAdminSchoolNP).ThenInclude(a => a!.SchoolAddress).FirstOrDefaultAsync(s => s.Id == actorId);
                        break;
                    case "Principal":
                        principal = await context.Principals.Include(p => p.PrincipalSchoolNP).ThenInclude(a => a!.SchoolAddress).FirstOrDefaultAsync(p => p.Id == actorId);
                        break;
                    case "Teacher":
                        teacher = await context.Teachers.Include(t => t.TeacherSchoolNP).ThenInclude(a => a!.SchoolAddress).FirstOrDefaultAsync(t => t.Id == actorId);
                        break;
                    case "Parent":
                        parent = await context.Parents.FirstOrDefaultAsync(p => p.Id == actorId);
                        break;
                    case "Learner":
                        learner = await context.Learners.FirstOrDefaultAsync(l => l.Id == actorId);
                        break;
                    default:
                        throw new($"The type {actor.GetType().Name} did not match any of the valid actor types.");
                }

                _returnDictionary["Success"] = true;
                if (admin != null)
                {
                    _returnDictionary["Result"] = admin;
                    return _returnDictionary;
                }
                else if (principal != null)
                {
                    _returnDictionary["Result"] = principal;
                    return _returnDictionary;
                }
                else if (teacher != null)
                {
                    _returnDictionary["Result"] = teacher;
                    return _returnDictionary;
                }
                else if (parent != null)
                {
                    _returnDictionary["Result"] = parent;
                    return _returnDictionary;
                }
                else if (learner != null)
                {
                    _returnDictionary["Result"] = learner;
                    return _returnDictionary;
                }
                else
                {
                    throw new($"Something went wrong, no actor with ID {actorId}.");
                }
                
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public static Dictionary<string, object> SaveFile(string name, string destinationFolder, IFormFile file)
        {
            Dictionary<string, object> _returnDictionary = [];
            try
            {

                string fileName = Path.GetExtension(file.FileName) == ".xlsx" ? name : name + $" - {Guid.NewGuid()}" + Path.GetExtension(file.FileName);
                using var fileStream = new FileStream($@"{ApplicationFilesPath}\{destinationFolder}\{fileName}", FileMode.Create);
                file.CopyTo(fileStream);

                _returnDictionary["Success"] = true;
                _returnDictionary["FileName"] = fileName;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }
        
        public static Dictionary<string, object> DeleteFile(string filePath)
        {
            Dictionary<string, object> _returnDictionary = [];
            try
            {
                if (!File.Exists(filePath))
                    throw new($"The file in the path '{filePath}' could not be found.");

                File.Delete(filePath);
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
        
        public static async Task<Dictionary<string, object>> RetrieveImageAsBase64Async(string fileName, string targetFolder, string? entity = null)
        {
            Dictionary<string, object> _returnDictionary = [];
            try
            {
                string filePath;
                if (entity == "School")
                {
                    filePath = fileName == "Default Pic.png" ? $@"{ApplicationFilesPath}\School Logos Folder\{fileName}" : $@"{ApplicationFilesPath}\{targetFolder}\{fileName}";
                }
                else
                {
                    filePath = fileName == "Default Pic.png" ? $@"{ApplicationFilesPath}\Profile Images Folder\{fileName}" : $@"{ApplicationFilesPath}\{targetFolder}\{entity}\{fileName}";
                }
                if (!File.Exists(filePath)) throw new FileNotFoundException($"The file in the path '{filePath}' could not be found.");

                _returnDictionary["Success"] = true;
                _returnDictionary["Result"] = Convert.ToBase64String(await File.ReadAllBytesAsync(filePath));
                _returnDictionary["ImageType"] = Path.GetExtension(filePath).Remove(0, 1);
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }
    }
}
