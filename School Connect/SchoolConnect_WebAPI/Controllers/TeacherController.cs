using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolConnect_DomainLayer.Data;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_RepositoryLayer.Interfaces;
using SchoolConnect_ServiceLayer.IServerSideServices;

namespace SchoolConnect_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController(ITeacherService teacherService) : ControllerBase
    {
        private readonly ITeacherService _teacherService = teacherService;
        private Dictionary<string, object> _returnDictionary = [];

        [HttpPost(nameof(BulkLoadTeachersFromExcel))]
        public IActionResult BulkLoadTeachersFromExcel(IFormFile file, long schoolId)
        {
            try
            {
                _returnDictionary = _teacherService.BulkLoadTeacherAsync(file, schoolId).Result;
                if (!(bool)_returnDictionary["Success"]) return BadRequest(_returnDictionary);
                return Ok(_returnDictionary);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(nameof(Create))]
        [Consumes("multipart/form-data")]
        public IActionResult Create(Teacher teacher)
        {
            try
            {
                _returnDictionary = _teacherService.CreateTeacherAsync(teacher).Result;
                if (!(bool)_returnDictionary["Success"])
                {
                    if (_returnDictionary.TryGetValue("Errors", out object? value))
                    {
                        var errorList = value as List<string>;
                        throw new(errorList!.First());
                    }

                    throw new(_returnDictionary["ErrorMessage"] as string);
                }

                return Ok(_returnDictionary);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(nameof(GetTeacherById))]
        public IActionResult GetTeacherById(long id)
        {
            try
            {
                _returnDictionary = _teacherService.GetByIdAsync(id).Result;
                if (!(bool)_returnDictionary["Success"]) return BadRequest(_returnDictionary["ErrorMessage"]);
                return Ok(_returnDictionary);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut(nameof(UpdateTeacher))]
        [Consumes("multipart/form-data")]
        public IActionResult UpdateTeacher(Teacher teacher)
        {
            try
            {
                _returnDictionary = _teacherService.UpdateAsync(teacher).Result;
                if (!(bool)_returnDictionary["Success"]) return BadRequest(_returnDictionary["ErrorMessage"]);
                return Ok(_returnDictionary);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
