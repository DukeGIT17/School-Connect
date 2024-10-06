using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_ServiceLayer.IServerSideServices;

namespace SchoolConnect_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParentController(IParentService parentService) : ControllerBase
    {
        private readonly IParentService _parentService = parentService;
        private Dictionary<string, object> _returnDictionary = [];

        [HttpPost(nameof(Create))]
        public IActionResult Create(Parent parent)
        {
            try
            {
                _returnDictionary = _parentService.CreateAsync(parent).Result;
                if (!(bool)_returnDictionary["Success"]) return BadRequest(_returnDictionary["ErrorMessage"]);
                return Ok(_returnDictionary["Success"]);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(nameof(GetParentById))]
        public IActionResult GetParentById(long id)
        {
            try
            {
                _returnDictionary = _parentService.GetById(id).Result;
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
