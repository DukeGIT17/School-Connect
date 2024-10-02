using Microsoft.AspNetCore.Mvc;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_ServiceLayer.IServerSideServices;

namespace SchoolConnect_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        private readonly ISchoolService _school;
        private Dictionary<string, object> _resultDictionary;

        public SchoolController(ISchoolService school)
        {
            _school = school;
            _resultDictionary = [];
        }

        [HttpGet(nameof(Schools))]
        public async Task<IActionResult> Schools() 
        {
            try
            {
                _resultDictionary = await _school.GetSchoolsAsync();

                if (!(bool)_resultDictionary["Success"])
                    return BadRequest(_resultDictionary["ErrorMessage"]);

                return Ok(_resultDictionary["Result"]);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(nameof(RegisterSchool))]
        public async Task<IActionResult> RegisterSchool(School school)
        {
            try
            {
                school.DateRegistered = DateTime.Now;
                school.SchoolAddress.SchoolID = (long)school.SystemAdminId!;
                _resultDictionary = await _school.RegisterSchoolAsync(school);

                if (!(bool)_resultDictionary["Success"])
                    return BadRequest(_resultDictionary["ErrorMessage"]);

                return Ok(_resultDictionary["Success"]);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
