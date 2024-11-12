using Microsoft.AspNetCore.Mvc;
using SchoolConnect_DomainLayer.Models;
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

        [HttpGet(nameof(GetAnnouncementByPrincipalId))]
        public IActionResult GetAnnouncementByPrincipalId(long principalId)
        {
            try
            {
                _returnDictionary = _announcementService.GetAnnouncementByPrincipalIdAsync(principalId).Result;
                if (!(bool)_returnDictionary["Success"]) return BadRequest(_returnDictionary["ErrorMessage"]);
                return Ok(_returnDictionary);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(nameof(GetAllAnnBySchool))]
        public IActionResult GetAllAnnBySchool(long schoolId)
        {
            try
            {
                _returnDictionary = _announcementService.GetAllAnnBySchool(schoolId).Result;
                if (!(bool)_returnDictionary["Success"]) return BadRequest(_returnDictionary["ErrorMessage"]);
                return Ok(_returnDictionary);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(nameof(GetAnnouncementsByTeacherId))]
        public IActionResult GetAnnouncementsByTeacherId(long id)
        {
            try
            {
                _returnDictionary = _announcementService.GetAnnouncementsByTeacherIdAsync(id).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                return Ok(_returnDictionary);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete(nameof(Delete))]
        public IActionResult Delete(int announcementId)
        {
            try
            {
                _returnDictionary = _announcementService.RemoveAsync(announcementId).Result;
                if (!(bool)_returnDictionary["Success"]) return BadRequest(_returnDictionary["ErrorMessage"] as string);
                return Ok(_returnDictionary);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut(nameof(Update))]
        public IActionResult Update(Announcement announcement)
        {
            try
            {
                _returnDictionary = _announcementService.UpdateAsync(announcement).Result;
                if (!(bool)_returnDictionary["Success"]) return BadRequest(_returnDictionary["ErrorMessage"]);
                return Ok(_returnDictionary);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(nameof(GetAnnouncementsByParentId))]
        public IActionResult GetAnnouncementsByParentId(long parentId)
        {
            try
            {
                _returnDictionary = _announcementService.GetAnnouncementsByParentIdAsync(parentId).Result;
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
