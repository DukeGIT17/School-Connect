using OfficeOpenXml;
using Microsoft.EntityFrameworkCore;
using SchoolConnect_DomainLayer.Data;
using SchoolConnect_DomainLayer.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing.Constraints;

namespace SchoolConnect_RepositoryLayer.CommonAction
{
    public static class CommonActions
    {
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
                using var fileStream = new FileStream($@"C:\Users\innoc\Desktop\Git Repo\School-Connect\School Connect\SchoolConnect_DomainLayer\Application Files\{destinationFolder}\{fileName}", FileMode.Create);
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
    }
}
