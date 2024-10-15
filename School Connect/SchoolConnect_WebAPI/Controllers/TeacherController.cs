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
    public class TeacherController(ITeacherService teacherService, SchoolConnectDbContext context, ISignInRepo signInRepo) : ControllerBase
    {
        private readonly ITeacherService _teacherService = teacherService;
        private readonly ISignInRepo _signInRepo = signInRepo;
        private readonly SchoolConnectDbContext _context = context;
        private Dictionary<string, object> _returnDictionary = [];

        [HttpDelete(nameof(DeleteTeacherById))]
        public IActionResult DeleteTeacherById(long id)
        {
            var teacher = _context.Teachers.FirstOrDefault(t => t.Id == id);
            if (teacher != null)
            {
                _signInRepo.RemoveUserAccountAsync(teacher.EmailAddress, "Teacher");
                _context.Remove(teacher);
                _context.SaveChanges();
                return Ok();
            }
            return BadRequest("Something went wrong.");
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
                _returnDictionary = _teacherService.GetById(id).Result;
                if (!(bool)_returnDictionary["Success"]) return BadRequest(_returnDictionary["ErrorMessage"]);
                return Ok(_returnDictionary["Result"]);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
