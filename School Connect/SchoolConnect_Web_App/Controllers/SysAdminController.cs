using Microsoft.AspNetCore.Mvc;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_Web_App.IServices;

namespace schoolconnect.Controllers
{
    public class SysAdminController : Controller
    {
        private readonly ISystemAdminService _systemAdminService;

        public SysAdminController(ISystemAdminService systemAdminService)
        {
            _systemAdminService = systemAdminService;
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
