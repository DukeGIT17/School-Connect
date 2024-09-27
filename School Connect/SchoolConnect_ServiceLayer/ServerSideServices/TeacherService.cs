using SchoolConnect_DomainLayer.Models;
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
                _returnDictionary = SharedValidationService.AttemptObjectValidation(teacher);
                if (!(bool)_returnDictionary["Success"]) return _returnDictionary;

                _returnDictionary = await _teacherRepository.CreateAsync(teacher);
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
