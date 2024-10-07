using OfficeOpenXml;
using Microsoft.EntityFrameworkCore;
using SchoolConnect_DomainLayer.Data;
using SchoolConnect_DomainLayer.Models;
using System.ComponentModel.DataAnnotations;

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
                        admin = await context.SystemAdmins.Include(s => s.SysAdminSchoolNP).FirstOrDefaultAsync(s => s.Id == actorId);
                        break;
                    case "Principal":
                        principal = await context.Principals.Include(p => p.PrincipalSchoolNP).FirstOrDefaultAsync(p => p.Id == actorId);
                        break;
                    case "Teacher":
                        teacher = await context.Teachers.Include(t => t.TeacherSchoolNP).FirstOrDefaultAsync(t => t.Id == actorId);
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

                if (admin != null)
                {
                    return new Dictionary<string, object>
                    {
                        {"Success", true },
                        {"Result",  admin }
                    };
                }
                else if (principal != null)
                {
                    return new Dictionary<string, object>
                    {
                        {"Success", true },
                        {"Result",  principal }
                    };
                }
                else if (teacher != null)
                {
                    return new Dictionary<string, object>
                    {
                        {"Success", true },
                        {"Result",  teacher }
                    };
                }
                else if (parent != null)
                {
                    return new Dictionary<string, object>
                    {
                        {"Success", true },
                        {"Result",  parent }
                    };
                }
                else if (learner != null)
                {
                    return new Dictionary<string, object>
                    {
                        {"Success", true },
                        {"Result",  learner }
                    };
                }
                else
                {
                    throw new($"Something went wrong, no actor with ID {actorId}.");
                }
                
            }
            catch (Exception ex)
            {
                return new Dictionary<string, object>
                {
                    {"Success", false },
                    {"ErrorMessage", ex.Message },
                };
            }
        }
    }
}
