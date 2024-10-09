using Microsoft.AspNetCore.Mvc;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_RepositoryLayer.Interfaces;
using SchoolConnect_ServiceLayer.IServerSideServices;

namespace SchoolConnect_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        private readonly ISchoolService _school;
        private Dictionary<string, object> _returnDictionary;

        public SchoolController(ISchoolService school)
        {
            _school = school;
            _returnDictionary = [];
        }

        [HttpGet(nameof(GetSchoolById))]
        public IActionResult GetSchoolById(long schoolId)
        {
            try
            {
                _returnDictionary = _school.GetSchoolByIdAsync(schoolId).Result;
                if (!(bool)_returnDictionary["Success"]) return BadRequest(_returnDictionary["ErrorMessage"]);
                return Ok(_returnDictionary);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(nameof(Schools))]
        public IActionResult Schools() 
        {
            try
            {
                _returnDictionary = _school.GetSchoolsAsync().Result;
                if (!(bool)_returnDictionary["Success"]) return BadRequest(_returnDictionary["ErrorMessage"]);
                return Ok(_returnDictionary);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(nameof(RegisterSchool))]
        public IActionResult RegisterSchool(School school)
        {
            try
            {
                school.DateRegistered = DateTime.Now;
                school.SchoolAddress.SchoolID = (long)school.SystemAdminId!;
                _returnDictionary = _school.RegisterSchoolAsync(school).Result;

                if (!(bool)_returnDictionary["Success"]) return BadRequest(_returnDictionary["ErrorMessage"]);
                return Ok(_returnDictionary);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(nameof(GetSchoolByAdminId))]
        public IActionResult GetSchoolByAdminId(long adminId)
        {
            try
            {
                _returnDictionary = _school.GetSchoolByAdminAsync(adminId).Result;
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
