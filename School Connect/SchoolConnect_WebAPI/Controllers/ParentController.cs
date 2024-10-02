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
        private Dictionary<string, object> _resultDictionary = [];

        [HttpPost(nameof(Create))]
        public IActionResult Create(Parent parent)
        {
            try
            {
                _resultDictionary = _parentService.CreateAsync(parent).Result;
                if (!(bool)_resultDictionary["Success"]) return BadRequest(_resultDictionary["ErrorMessage"]);
                return Ok(_resultDictionary["Success"]);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
