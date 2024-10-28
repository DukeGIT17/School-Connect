using SchoolConnect_DomainLayer.Models;
using SchoolConnect_ServiceLayer.IServerSideServices;
using SchoolConnect_RepositoryLayer.Interfaces;
using SchoolConnect_RepositoryLayer.CommonAction;

namespace SchoolConnect_ServiceLayer.ServerSideServices
{
    public class AnnouncementService(IAnnouncement announcementRepo) : IAnnouncementService
    {
        private readonly IAnnouncement _announcementRepo = announcementRepo;
        private Dictionary<string, object> _returnDictionary = [];

        public async Task<Dictionary<string, object>> CreateAnnouncementAsync(Announcement announcement)
        {
            try
            {
                _returnDictionary = CommonActions.AttemptObjectValidation(announcement);
                if (!(bool)_returnDictionary["Success"])
                    throw new(_returnDictionary["ErrorMessage"] as string);

                return await _announcementRepo.CreateAsync(announcement);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetAllAnnBySchool(long schoolId)
        {
            try
            {
                return await _announcementRepo.GetAllAnnBySchoolAsync(schoolId);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetAnnouncementByIdAsync(int announcementId)
        {
            try
            {
                return await _announcementRepo.GetAnnByIdAsync(announcementId);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetAnnouncementByPrincipalIdAsync(long principalId)
        {
            try
            {
                return await _announcementRepo.GetAnnouncementsByPrincipalIdAsync(principalId);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetAnnouncementsByTeacherIdAsync(long teacherId)
        {
            try
            {
                return await _announcementRepo.GetAnnouncementsByTeacherIdAsync(teacherId);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> RemoveAsync(int announcementId)
        {
            try
            {
                return await _announcementRepo.RemoveAsync(announcementId);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> UpdateAsync(Announcement announcement)
        {
            try
            {
                return await _announcementRepo.UpdateAnnouncementAsync(announcement);
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
