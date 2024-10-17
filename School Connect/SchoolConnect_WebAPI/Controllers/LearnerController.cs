using Microsoft.AspNetCore.Mvc;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_ServiceLayer.IServerSideServices;

namespace SchoolConnect_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LearnerController(ILearnerService learnerService) : ControllerBase
    {
        private readonly ILearnerService _learnerService = learnerService;
        private Dictionary<string, object> _returnDictionary = [];

        [HttpPost(nameof(LoadLearnersFromExcel))]
        [Consumes("multipart/form-data")]
        public IActionResult LoadLearnersFromExcel([FromForm] IFormFile file, long schoolId)
        {
            try
            {
                _returnDictionary = _learnerService.LoadLearners(file, schoolId).Result;
                if (!(bool)_returnDictionary["Success"]) return BadRequest(_returnDictionary["ErrorMessage"]);
                return Ok(_returnDictionary);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(nameof(Create))]
        [Consumes("multipart/form-data")]
        public IActionResult Create([FromForm] Learner learner)
        {
            try
            {
                _returnDictionary = _learnerService.CreateAsync(learner).Result;
                if (!(bool)_returnDictionary["Success"])
                {
                    if (_returnDictionary.TryGetValue("ErrorMessage", out object? value))
                        throw new(value as string);
                    else
                    {
                        string errorStrings = "";
                        List<string>? errors = _returnDictionary["Errors"] as List<string> ?? throw new("Something went wrong, could not acquired the list of errors.");
                        foreach (var error in errors)
                            errorStrings += error + "\n\n";

                        throw new(errorStrings);
                    }
                }
                return Ok(_returnDictionary);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(nameof(GetLearnerById))]
        public IActionResult GetLearnerById(long id)
        {
            try
            {
                _returnDictionary = _learnerService.GetById(id).Result;
                if (!(bool)_returnDictionary["Success"]) return BadRequest(_returnDictionary["ErrorMessage"]);
                return Ok(_returnDictionary);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet(nameof(GetLearnerByIdNo))]
        public IActionResult GetLearnerByIdNo(string idNo)
        {
            try
            {
                _returnDictionary = _learnerService.GetByIdNo(idNo).Result;
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
