using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_ServiceLayer.IServerSideServices;

namespace SchoolConnect_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrincipalController : ControllerBase
    {
        private readonly IPrincipalService _principalService;
        private Dictionary<string, object> _returnDictionary;

        public PrincipalController(IPrincipalService principalService)
        {
            _principalService = principalService;
            _returnDictionary = [];
        }

        [HttpPost(nameof(Create))]
        public IActionResult Create(Principal principal)
        {
            //TODO: On the frontend, don't forget to retrieve the school ID value on the School Admin navigation property and asign it to all actors they
            // register onto the system. This is to ensure that the admin can only register actors to their school as well as giving us the ability to use
            // the value to search for school's data in the database.
            try
            {
                if (principal == null)
                    throw new Exception("Provided principal object is null.");

                _returnDictionary = _principalService.Create(principal).Result;
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
