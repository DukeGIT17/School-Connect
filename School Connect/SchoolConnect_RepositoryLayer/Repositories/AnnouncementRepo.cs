using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SchoolConnect_DomainLayer.Data;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_RepositoryLayer.Interfaces;

namespace SchoolConnect_RepositoryLayer.Repositories
{
    public class AnnouncementRepo(SchoolConnectDbContext context) : IAnnouncement
    {
        private readonly SchoolConnectDbContext _context = context;
        private Dictionary<string, object>  _returnDictionary = [];

        public async Task<Dictionary<string, object>> CreateAsync(Announcement announcement)
        {
            try
            {
                announcement.DateCreated = DateTime.Now;

                if (announcement.TeacherID != null)
                {
                    var teacher = _context.Teachers.FirstOrDefaultAsync(t => t.Id == announcement.TeacherID).Result 
                        ?? throw new($"Something went wrong, could not find a teacher with the specified ID. Please contact the administrator for assistance.");
                }
                else if (announcement.PrincipalID != null)
                {
                    var principal = _context.Principals.Include(s => s.PrincipalSchoolNP).FirstOrDefaultAsync(p => p.Id == announcement.PrincipalID).Result
                        ?? throw new($"Something went wrong, could not find a principal with the specified ID. Please contact the administrator for assistance.");
                }
                else
                    throw new("Something went wrong, both teacher and principal IDs were null.");

                await _context.AddAsync(announcement);
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

        public async Task<Dictionary<string, object>> GetAnnByIdAsync(int announcementId)
        {
            try
            {
                var ann = await _context.Announcements.FirstOrDefaultAsync(a => a.AnnouncementId == announcementId);
                if (ann == null) throw new($"Could not find an announcement with the specified ID");

                _returnDictionary["Success"] = true;
                _returnDictionary["Result"] = ann;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetAllAnnBySchoolAsync(long schoolId)
        {
            try
            {
                var school = await _context.Schools.Include(a => a.SchoolAnnouncementsNP).FirstOrDefaultAsync(s => s.Id == schoolId);
                if (school is null) throw new("Could not find a school with the specified ID.");

                if (school.SchoolAnnouncementsNP is null) throw new($"Could not find any announcements associated with {school.Name} {school.Type} School");

                var announcements = school.SchoolAnnouncementsNP.Where(a => a.TimeToPost is null || a.TimeToPost.Value <= DateTime.Now).ToList();
                announcements.ForEach(x => x.AnnouncementSchoolNP!.SchoolAnnouncementsNP = null);


                _returnDictionary["Success"] = true;
                _returnDictionary["Result"] = announcements;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message + "\nInner Exception: " + ex.InnerException;
                return _returnDictionary;
            }
        }

        public Task<Dictionary<string, object>> GetAnnouncementsById(int announcementId)
        {
            throw new NotImplementedException();
        }

        public async Task<Dictionary<string, object>> GetAnnouncementsByTeacherIdAsync(long teacherId)
        {
            try
            {
                var teacher = await _context.Teachers
                    .AsNoTracking()
                    .Include(a => a.GroupsNP)!
                    .ThenInclude(a => a.GroupNP)
                    .Include(a => a.TeacherSchoolNP)
                    .ThenInclude(a => a.SchoolAnnouncementsNP)
                    .FirstOrDefaultAsync(s => s.Id == teacherId);
                if (teacher is null) throw new("Could not find a teacher with the specified ID.");

                teacher.TeacherSchoolNP!.SchoolTeachersNP = null;
                var announcements = teacher.TeacherSchoolNP!.SchoolAnnouncementsNP!.ToList();
                var teacherGroups = teacher.GroupsNP!.ToList();

                List<Announcement> teacherAnns = [];
                foreach (var announcement in announcements)
                {
                    foreach (var group in teacherGroups)
                    {
                        if (announcement.Recipients.Contains(group.GroupNP.GroupName))
                        {
                            teacherAnns.Add(announcement);
                        }
                    }
                    announcement.AnnouncementSchoolNP = null;
                }

                _returnDictionary["Success"] = true;
                _returnDictionary["Result"] = teacherAnns;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message + "\n\nInner Exception: " + ex.InnerException;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> RemoveAsync(int announcementId)
        {
            try
            {
                var announcement = await _context.Announcements.FirstOrDefaultAsync(a => a.AnnouncementId == announcementId);
                if (announcement is null) throw new("Could not find an announcement with the specified ID.");

                _context.Remove(announcement);
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

        public async Task<Dictionary<string, object>> UpdateAnnouncementAsync(Announcement announcement)
        {
            try
            {
                var ann = await _context.Announcements.AsNoTracking().FirstOrDefaultAsync(a => a.AnnouncementId == announcement.AnnouncementId);
                if (ann is null) throw new("Could not find an announcement with the specified ID.");

                _context.Update(announcement);
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

        public async Task<Dictionary<string, object>> GetAnnouncementsByPrincipalIdAsync(long principalId)
        {
            try
            {

                var principal = await _context.Principals.FirstOrDefaultAsync(p => p.Id == principalId);
                if (principal is null) throw new("Could not find a principal with the specified ID.");

                var announcements = _context.Announcements.Where(a => a.SchoolID == principal.SchoolID);
                if (announcements.IsNullOrEmpty()) throw new("This principal does not have any announecements associated with them.");

                _returnDictionary["Success"] = true;
                _returnDictionary["Result"] = announcements;
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
