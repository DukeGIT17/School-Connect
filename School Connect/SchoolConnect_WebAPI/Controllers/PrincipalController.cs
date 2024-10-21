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
        [Consumes("multipart/form-data")]
        public IActionResult Create([FromForm] Principal principal)
        {
            try
            {
                _returnDictionary = _principalService.Create(principal).Result;
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

        [HttpGet(nameof(GetPrincipalById))]
        public IActionResult GetPrincipalById(long id)
        {
            try
            {
                _returnDictionary = _principalService.GetById(id).Result;
                if (!(bool)_returnDictionary["Success"]) return BadRequest(_returnDictionary["ErrorMessage"]);
                return Ok(_returnDictionary);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPut(nameof(UpdatePrincipal))]
        [Consumes("multipart/form-data")]
        public IActionResult UpdatePrincipal(Principal principal)
        {
            try
            {
                _returnDictionary = _principalService.UpdateAsync(principal).Result;
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
