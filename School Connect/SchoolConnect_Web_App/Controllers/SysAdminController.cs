using Microsoft.AspNetCore.Mvc;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_Web_App.IServices;
using SchoolConnect_Web_App.Models;

namespace SchoolConnect_Web_App.Controllers
{
    public class SysAdminController : Controller
    {
        private readonly ISystemAdminService _systemAdminService;
        private Dictionary<string, object> _resultDictionary;

        public SysAdminController(ISystemAdminService systemAdminService)
        {
            _systemAdminService = systemAdminService;
            _resultDictionary = [];
        }

        [HttpGet]
        public IActionResult SysAdminLandingPage(long id)
        {
            try
            {
                _resultDictionary = _systemAdminService.GetAdminById(id);
                if (!(bool)_resultDictionary["Success"]) throw new(_resultDictionary["ErrorMessage"] as string);
                return View(_resultDictionary["Result"] as SysAdmin);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n" + ex.Message.ToUpper() + "\n\n");
                return RedirectToAction("Index", "Home");
            }
        }
        
        public IActionResult SchoolReg()
        {
            return View();
        }

        public IActionResult RolesReg()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SysAdminViewProfile(long id)
        { 
            try
            {
                _resultDictionary = _systemAdminService.GetAdminById(id);
                if (!(bool)_resultDictionary["Success"]) throw new(_resultDictionary["ErrorMessage"] as string);

                var admin = _resultDictionary["Admin"] as SysAdmin;
                var school = _resultDictionary["School"] as School;

                SystemAdminProfileModel model = new()
                {
                    Admin = admin!,
                    EmisNumber = school!.EmisNumber
                };

                return View(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n" + ex.Message.ToUpper() + "\n\n");
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
