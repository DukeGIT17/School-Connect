using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_RepositoryLayer.Interfaces;
using System.Reflection.Metadata.Ecma335;

namespace SchoolConnect_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SysAdminController : ControllerBase
    {
        private readonly ISysAdmin _systemAdmin;

        public SysAdminController(ISysAdmin systemAdmin)
        {
            _systemAdmin = systemAdmin;
        }

        [HttpGet(nameof(GetSystemAdminById))]
        public async Task<IActionResult> GetSystemAdminById(long id)
        {
            try
            {
                var admin = await _systemAdmin.GetAdminById(id);
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

        [HttpGet(nameof(GetSystemAdminByStaffNr))]
        public async Task<IActionResult> GetSystemAdminByStaffNr(long staffNr)
        {
            try
            {
                var admin = await _systemAdmin.GetAdminByStaffNr(staffNr);
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
                var result = await _systemAdmin.Update(systemAdmin);
                var success = result.GetValueOrDefault("Success") ?? throw new Exception("Could not find the Success Key in the System Admin Repository. Please contact Lukhanyo. Ungacofacofi!!!! :(");

                if (!(bool)success)
                    return BadRequest(result.GetValueOrDefault("ErrorMessage") as string ?? "Something went wrong, operationw as unsuccessful.");

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
