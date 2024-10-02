using SchoolConnect_DomainLayer.Models;
using SchoolConnect_RepositoryLayer.Interfaces;
using SchoolConnect_ServiceLayer.IServerSideServices;

namespace SchoolConnect_ServiceLayer.ServerSideServices
{
    public class AdminService : ISystemAdminService
    {
        private readonly ISysAdmin _sysAdminRepo;
        private Dictionary<string, object> _returnDictionary;

        public AdminService(ISysAdmin sysAdminRepo)
        {
            _sysAdminRepo = sysAdminRepo;
            _returnDictionary = [];
        }

        public async Task<Dictionary<string, object>> GetAdminById(long id)
        {
            try
            {
                return _returnDictionary = id < 1 
                    ? throw new Exception($"Invalid ID. Provided ID '{id}' was less than 1 (One).") 
                    : await _sysAdminRepo.GetAdminById(id);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }
        
        public async Task<Dictionary<string, object>> GetAdminByStaffNr(long staffNr)
        {
            try
            {
                if (staffNr.ToString().Length < 5 || staffNr.ToString().Length >= 8)
                    throw new Exception($"Invalid Staff Number. Staff should be between 5 and 8 digits in length.");

                _returnDictionary = await _sysAdminRepo.GetAdminByStaffNr(staffNr);
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> UpdateSystemAdmin(SysAdmin admin)
        {
            try
            {
                _returnDictionary = SharedValidationService.AttemptObjectValidation(admin);
                if (!(bool)_returnDictionary["Success"]) return _returnDictionary;

                _returnDictionary = await _sysAdminRepo.Update(admin);
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
