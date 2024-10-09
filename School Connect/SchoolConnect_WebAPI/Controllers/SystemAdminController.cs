using Microsoft.AspNetCore.Mvc;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_ServiceLayer.IServerSideServices;

namespace SchoolConnect_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemAdminController : ControllerBase
    {
        private readonly ISystemAdminService _systemAdminService;
        private Dictionary<string, object> _returnDictionary;

        public SystemAdminController(ISystemAdminService systemAdminService)
        {
            _systemAdminService = systemAdminService;
            _returnDictionary = [];
        }

        [HttpGet(nameof(GetSystemAdminById))]
        public async Task<IActionResult> GetSystemAdminById(long id)
        {
            try
            {
                _returnDictionary = await _systemAdminService.GetAdminById(id);
                if (!(bool)_returnDictionary["Success"]) return BadRequest(_returnDictionary["ErrorMessage"]);
                return Ok(_returnDictionary);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(nameof(GetSystemAdminByStaffNr))]
        public async Task<IActionResult> GetSystemAdminByStaffNr(long staffNr)
        {
            try
            {
                _returnDictionary = await _systemAdminService.GetAdminByStaffNr(staffNr);
                if (!(bool)_returnDictionary["Success"]) return BadRequest(_returnDictionary["ErrorMessage"]);
                return Ok(_returnDictionary);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPut(nameof(UpdateSystemAdmin))]
        public async Task<IActionResult> UpdateSystemAdmin(SysAdmin systemAdmin)
        {
            try
            {
                _returnDictionary = await _systemAdminService.UpdateSystemAdmin(systemAdmin);
                if (!(bool)_returnDictionary["Success"])
                {
                    List<string> error = _returnDictionary.GetValueOrDefault("Errors") as List<string>
                        ?? throw new Exception(_returnDictionary["ErrorMessage"] as string
                        ?? "Something could went wrong, could not acquire error messages.");
                    return BadRequest(error);
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
