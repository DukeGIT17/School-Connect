using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_RepositoryLayer.Interfaces;

namespace SchoolConnect_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        private readonly ISchool _school;

        public SchoolController(ISchool school)
        {
            _school = school;
        }

        [HttpGet(nameof(Schools))]
        public async Task<IActionResult> Schools() 
        {
            var schools = await _school.GetSchools();
            var success = schools.GetValueOrDefault("Success") ?? throw new Exception("Could not find the Success key.");
            
            if (success == null)
                return BadRequest("Dictionary Key: 'Success', not found. Pleave review the SchoolRepository class or have Lukhanyo review it.");
            else if (!(bool)success)
                return BadRequest(schools.GetValueOrDefault("ErrorMessage"));
            else
                return Ok(schools.GetValueOrDefault("Result") as List<School>);
        }

        [HttpPost(nameof(RegisterSchool))]
        public async Task<IActionResult> RegisterSchool(School school)
        {
            if (school != null)
            {
                var result = await _school.RegisterSchool(school);
                var success = result.GetValueOrDefault("Success");

                if (success == null)
                    return BadRequest("Dictionary Key: 'Success', not found! Pleave review the SchoolRepository class or have Lukhanyo review it.");
                
                if (!(bool)success)
                    return BadRequest(result.GetValueOrDefault("ErrorMessage"));

                return Ok("Success");
            }

            return BadRequest("The school parameter that was passed in is null.");
        }
    }
}
