using SchoolConnect_DomainLayer.Data;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_RepositoryLayer.Interfaces;
using System.Linq.Expressions;

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

        public Task<Dictionary<string, object>> Get(int announcementId)
        {
            throw new NotImplementedException();
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
