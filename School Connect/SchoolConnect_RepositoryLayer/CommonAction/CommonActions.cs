using OfficeOpenXml;
using Microsoft.EntityFrameworkCore;
using SchoolConnect_DomainLayer.Data;
using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_RepositoryLayer.CommonAction
{
    public static class CommonActions
    {
        
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
                        admin = await context.SystemAdmins.FirstOrDefaultAsync(s => s.Id == actorId);
                        break;
                    case "Principal":
                        principal = await context.Principals.FirstOrDefaultAsync(p => p.Id == actorId);
                        break;
                    case "Teacher":
                        teacher = await context.Teachers.FirstOrDefaultAsync(t => t.Id == actorId);
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
