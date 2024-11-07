using Microsoft.AspNetCore.Http;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_RepositoryLayer.Interfaces;
using SchoolConnect_ServiceLayer.IServerSideServices;
using static SchoolConnect_RepositoryLayer.CommonAction.CommonActions;

namespace SchoolConnect_ServiceLayer.ServerSideServices
{
    public class TeacherService(ITeacher teacherRepository) : ITeacherService
    {
        private readonly ITeacher _teacherRepository = teacherRepository;
        private Dictionary<string, object> _returnDictionary = [];

        public async Task<Dictionary<string, object>> BulkLoadTeacherAsync(IFormFile teacherFile, long schoolId)
        {
            try
            {
                return await _teacherRepository.BulkLoadTeacherFromExcel(teacherFile, schoolId);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> CreateTeacherAsync(Teacher teacher)
        {
            try
            {
                _returnDictionary = AttemptObjectValidation(teacher);
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

        public async Task<Dictionary<string, object>> GetAttendanceRecordsByTeacherAsync(long teacherId)
        {
            try
            {
                return await _teacherRepository.GetAttendanceRecordsByTeacherAsync(teacherId);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetByIdAsync(long id)
        {
            try
            {
                return id < 1 ?
                    throw new($"The provided id '{id}' is less than one. Please provide a valid id.") :
                    await _teacherRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public Task<Dictionary<string, object>> GetByStaffNr(string staffNr)
        {
            throw new NotImplementedException();
        }

        public async Task<Dictionary<string, object>> GetGradesByTeacherAsync(long teacherId)
        {
            try
            {
                return await _teacherRepository.GetGradesByTeacherAsync(teacherId);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetParentsByTeacherClassesAsync(long teacherId)
        {
            try
            {
                return await _teacherRepository.GetParentsByTeacherClassesAsync(teacherId);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetTeacherByEmailAddressAsync(string email)
        {
            try
            {
                return await _teacherRepository.GetTeacherByEmailAddressAsync(email);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetTeachersBySchoolAsync(long schoolId)
        {
            try
            {
                return await _teacherRepository.GetTeachersBySchoolAsync(schoolId);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> MarkClassAttendanceAsync(IEnumerable<Attendance> attendanceRecords)
        {
            try
            {
                return await _teacherRepository.MarkClassAttendanceAsync(attendanceRecords);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> UpdateClassAllocationAsync(Teacher teacher)
        {
            try
            {
                return await _teacherRepository.UpdateClassAllocationAsync(teacher);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> UpdatePersonalInfoAsync(Teacher teacher)
        {
            try
            {
                return await _teacherRepository.UpdatePersonalInfoAsync(teacher);
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
