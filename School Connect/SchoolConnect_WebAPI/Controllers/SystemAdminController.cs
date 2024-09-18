using Microsoft.AspNetCore.Mvc;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_ServiceLayer.ISystemAdminServices;

namespace SchoolConnect_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemAdminController : ControllerBase
    {
        private readonly ISystemAdminService _systemAdminService;

        public SystemAdminController(ISystemAdminService systemAdminService)
        {
            _systemAdminService = systemAdminService;
        }

        [HttpGet(nameof(GetSystemAdminById))]
        public async Task<IActionResult> GetSystemAdminById(long id)
        {
            try
            {
                var result = await _systemAdminService.GetAdminById(id);
                var success = result["Success"];

                if (!(bool)success)
                    return BadRequest(result["ErrorMessage"]);

                return Ok(result);
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
                var admin = await _systemAdminService.GetAdminByStaffNr(staffNr);
                var success = admin["Success"];

                if (!(bool)success)
                    return BadRequest(admin["ErrorMessage"]);

                var result = admin["Result"] as SysAdmin;
                return Ok(result);
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
                var result = await _systemAdminService.UpdateSystemAdmin(systemAdmin);
                var success = result["Success"];

                if (!(bool)success)
                    return BadRequest(result["ErrorMessage"]);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
