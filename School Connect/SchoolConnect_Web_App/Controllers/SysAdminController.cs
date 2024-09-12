using Microsoft.AspNetCore.Mvc;

namespace schoolconnect.Controllers
{
    public class SysAdminController : Controller
    {
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
