using Microsoft.EntityFrameworkCore;
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
                    var principal = _context.Principals.FirstOrDefaultAsync(p => p.Id == announcement.PrincipalID).Result
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
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetAnnById(int announcementId)
        {
            try
            {
                var ann = await _context.Announcements.FirstOrDefaultAsync(a => a.AnnouncementId == announcementId);
                if (ann == null)
                    throw new($"Could not find an announcement with the specified ID");

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

        public Task<Dictionary<string, object>> GetAllRelevantAnnouncements()
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, object>> GetAnnouncementsById(int announcementId)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, object>> GetAnnouncementsByTeacher(long teacherId)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, object>> Remove(int announcementId)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, object>> Update(Announcement announcement)
        {
            throw new NotImplementedException();
        }
    }
}
