using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_ServiceLayer.IServerSideServices;

namespace SchoolConnect_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController(ITeacherService teacherService) : ControllerBase
    {
        private readonly ITeacherService _teacherService = teacherService;
        private Dictionary<string, object> _returnDictionary = [];

        [HttpPost(nameof(Create))]
        public IActionResult Create(Teacher teacher)
        {
            try
            {
                _returnDictionary = _teacherService.CreateTeacherAsync(teacher).Result;
                if (!(bool)_returnDictionary["Success"])
                {
                    if (_returnDictionary.ContainsKey("Errors"))
                    {
                        var errorList = _returnDictionary["Errors"] as List<string>;
                        throw new Exception(errorList!.First());
                    }

                    throw new Exception(_returnDictionary["ErrorMessage"] as string);
                }

                return Ok(_returnDictionary["Success"]);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
