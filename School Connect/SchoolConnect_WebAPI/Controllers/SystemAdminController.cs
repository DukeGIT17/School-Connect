using Microsoft.AspNetCore.Http;
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

        [HttpPost(nameof(CreateAdmin))]
        public async Task<IActionResult> CreateAdmin(SysAdmin admin)
        {
            try
            {
                var resultDictionary = await _systemAdminService.CreateAdmin(admin);
                var success = resultDictionary["Success"];

                if (!(bool)success)
                    return BadRequest(resultDictionary["ErrorMessage"] as string ?? "Operation failed.");

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(nameof(GetSystemAdminById))]
        public async Task<IActionResult> GetSystemAdminById(long id)
        {
            try
            {
                var result = await _systemAdminService.GetAdminById(id);
                var success = result["Success"];

                if (!(bool)success)
                    return BadRequest(result);

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
                var success = admin.GetValueOrDefault("Success") ?? throw new Exception("Could not find the Success Key in the System Admin Repository. Please contact Lukhanyo. Ungacofacofi!!!! :(");

                if (!(bool)success)
                    return BadRequest(admin.GetValueOrDefault("ErrorMessage") as string ?? "Something went wrong, operation was unsuccessful.");

                var result = admin.GetValueOrDefault("Result") as SysAdmin ?? throw new Exception("Something went wrong! Please contact Lukhanyo. Ungacofacofi!!!! :(");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost(nameof(UpdateSystemAdmin))]
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
