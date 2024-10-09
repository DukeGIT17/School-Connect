using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_RepositoryLayer.Interfaces;
using SchoolConnect_ServiceLayer.IServerSideServices;

namespace SchoolConnect_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementController(IAnnouncementService announcementService) : ControllerBase
    {
        private readonly IAnnouncementService _announcementService = announcementService;
        private Dictionary<string, object> _returnDictionary = [];

        [HttpPost(nameof(Create))]
        public IActionResult Create(Announcement announcement)
        {
            try
            {
                _returnDictionary = _announcementService.CreateAnnouncementAsync(announcement).Result;
                if (!(bool)_returnDictionary["Success"]) return BadRequest(_returnDictionary["ErrorMessage"]);
                return Ok(_returnDictionary);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(nameof(GetAnnouncementById))]
        public IActionResult GetAnnouncementById(int annId)
        {
            try
            {
                _returnDictionary = _announcementService.GetAnnouncementByIdAsync(annId).Result;
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
