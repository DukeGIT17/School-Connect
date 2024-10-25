using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_ServiceLayer.IServerSideServices;
using System.Runtime.InteropServices.Marshalling;

namespace SchoolConnect_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParentController(IParentService parentService) : ControllerBase
    {
        private readonly IParentService _parentService = parentService;
        private Dictionary<string, object> _returnDictionary = [];

        [HttpPost(nameof(BatchLoadParentsFromExcel))]
        public IActionResult BatchLoadParentsFromExcel(IFormFile file)
        {
            try
            {
                _returnDictionary = _parentService.BatchLoadParentsAsync(file).Result;
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
        public IActionResult Create([FromForm] Parent parent)
        {
            try
            {
                _returnDictionary = _parentService.CreateAsync(parent).Result;
                if (!(bool)_returnDictionary["Success"]) return BadRequest(_returnDictionary["ErrorMessage"]);
                return Ok(_returnDictionary);
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
                return Ok(_returnDictionary);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut(nameof(Update))]
        [Consumes("multipart/form-data")]
        public IActionResult Update(Parent parent)
        {
            try
            {
                _returnDictionary = _parentService.UpdateAsync(parent).Result;
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
