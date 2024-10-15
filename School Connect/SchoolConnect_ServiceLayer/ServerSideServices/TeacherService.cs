using SchoolConnect_DomainLayer.Models;
using SchoolConnect_RepositoryLayer.CommonAction;
using SchoolConnect_RepositoryLayer.Interfaces;
using SchoolConnect_ServiceLayer.IServerSideServices;
using System.ComponentModel.DataAnnotations;

namespace SchoolConnect_ServiceLayer.ServerSideServices
{
    public class TeacherService(ITeacher teacherRepository) : ITeacherService
    {
        private readonly ITeacher _teacherRepository = teacherRepository;
        private Dictionary<string, object> _returnDictionary = [];

        public async Task<Dictionary<string, object>> CreateTeacherAsync(Teacher teacher)
        {
            try
            {
                _returnDictionary = CommonActions.AttemptObjectValidation(teacher);
                if (!(bool)_returnDictionary["Success"]) return _returnDictionary;
                return await _teacherRepository.CreateAsync(teacher);
            }
            catch (Exception ex) 
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetById(long id)
        {
            try
            {
                return id < 1 ?
                    throw new($"The provided id '{id}' is less than one. Please provide a valid id.") :
                    await _teacherRepository.GetById(id);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = true;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public Task<Dictionary<string, object>> GetByStaffNr(string staffNr)
        {
            throw new NotImplementedException();
        }
    }
}
