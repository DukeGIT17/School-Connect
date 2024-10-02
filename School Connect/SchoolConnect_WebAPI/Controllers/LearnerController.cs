using Microsoft.AspNetCore.Http;
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

        [HttpPost(nameof(Create))]
        public IActionResult Create(Learner learner)
        {
            try
            {
                _returnDictionary = _learnerService.CreateAsync(learner).Result;
                if (!(bool)_returnDictionary["Success"])
                {
                    var itIsAnErrorMessage = _returnDictionary.GetValueOrDefault("ErrorMessage") != null;
                    if (itIsAnErrorMessage)
                        throw new(_returnDictionary["ErrorMessage"] as string);
                    else
                    {
                        string errorStrings = "";
                        List<string>? errors = _returnDictionary["Errors"] as List<string> ?? throw new("Something went wrong, could not acquired the list of errors.");
                        foreach (var error in errors)
                            errorStrings += error + "\n\n";

                        throw new(errorStrings);
                    }
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
