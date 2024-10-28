using Microsoft.IdentityModel.Tokens;
using SchoolConnect_DomainLayer.Data;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_RepositoryLayer.Interfaces;

namespace SchoolConnect_RepositoryLayer.Repositories
{
    public class GroupRepository(SchoolConnectDbContext context) : IGroupRepo
    {
        private readonly SchoolConnectDbContext _context = context;
        private Dictionary<string, object> _returnDictionary = [];

        public async Task<Dictionary<string, object>> AddToGroup(string actorId, long schoolId, string groupName)
        {
            try
            {
                var groups = _context.Groups.Where(g => g.SchoolID == schoolId);
                if (groups.IsNullOrEmpty())
                    throw new("No groups for this school could be found in the groups table.");

                var targetGroup = groups.FirstOrDefault(g => g.GroupName == groupName)
                    ?? throw new($"Could not find the '{groupName}' group in this school's groups.");

                targetGroup.GroupMemberIDs.Add(actorId);

                _context.Update(targetGroup);
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

        public Dictionary<string, object> AddTeacherToGroup(ref Teacher teacher, long schoolId, string groupName)
        {
            try
            {
                var groups = _context.Groups.Where(g => g.SchoolID == schoolId);
                if (groups.IsNullOrEmpty())
                    throw new("No groups for this school could be found in the groups table.");

                var targetGroup = groups.FirstOrDefault(g => g.GroupName == groupName)
                    ?? throw new($"Could not find the '{groupName}' group in this school's groups.");

                GroupTeacher gt = new()
                {
                    GroupId = targetGroup.GroupId,
                    TeacherId = teacher.Id,
                    GroupNP = targetGroup
                };

                if (teacher.GroupsNP is not null)
                {
                    gt.GroupNP.GroupMemberIDs.Add(teacher.StaffNr);
                    teacher.GroupsNP.Add(gt);
                }
                else
                {
                    gt.GroupNP.GroupMemberIDs.Add(teacher.StaffNr);
                    teacher.GroupsNP = [];
                    teacher.GroupsNP.Add(gt);
                }

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

        public Dictionary<string, object> AddParentToGroup(Parent parent, long schoolId, string groupName)
        {
            try
            {
                var groups = _context.Groups.Where(g => g.SchoolID == schoolId);
                if (groups.IsNullOrEmpty()) throw new("No groups for this school could be found in the groups table.");

                var targetGroup = groups.FirstOrDefault(g => g.GroupName == groupName)
                    ?? throw new($"Could not find the '{groupName}' group in this school's groups.");

                GroupParent gp = new()
                {
                    GroupId = targetGroup.GroupId,
                    ParentId = parent.Id,
                    GroupNP = targetGroup
                };

                if (parent.GroupsNP is not null)
                {
                    gp.GroupNP.GroupMemberIDs.Add(parent.IdNo);
                    parent.GroupsNP.Add(gp);
                }
                else
                {
                    gp.GroupNP.GroupMemberIDs.Add(parent.IdNo);
                    parent.GroupsNP = [];
                    parent.GroupsNP.Add(gp);
                }

                _returnDictionary["Success"] = true;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message + "\n\nInner Exception: " + ex.InnerException;
                return _returnDictionary;
            }
        }
    }
}
