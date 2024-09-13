using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_ServiceLayer.IServices;

namespace schoolconnect.Controllers
{
    public class SysAdminController : Controller
    {
        private readonly ISystemAdminService _systemAdminService;

        public SysAdminController(ISystemAdminService systemAdminService)
        {
            _systemAdminService = systemAdminService;
        }

        [HttpGet]
        public IActionResult RegisterAdmin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAdmin(SysAdmin admin)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _systemAdminService.CreateAdmin(admin);
                    var success = result.GetValueOrDefault("Success") ?? throw new Exception("Could not find the Success Key");

                    if (!(bool)success)
                    {
                        ModelState.AddModelError(string.Empty, result.GetValueOrDefault("ErrorMessage") as string ?? "Something went wrong");
                        return View(admin);
                    }

                    return RedirectToAction(nameof(SysAdminLandingPage));
                }
                return View(admin);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(admin);
            }
        }

        public IActionResult SysAdminLandingPage()
        {
            return View();
        }
        
        public IActionResult SchoolReg()
        {
            return View();
        }

        public IActionResult RolesReg()
        {
            return View();
        }
    }
}
