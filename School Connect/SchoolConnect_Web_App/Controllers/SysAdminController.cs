using Microsoft.AspNetCore.Mvc;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_Web_App.IServices;
using SchoolConnect_Web_App.Models;

namespace SchoolConnect_Web_App.Controllers
{
    public class SysAdminController : Controller
    {
        private readonly ISystemAdminService _systemAdminService;
        private readonly ISchoolService _schoolService;
        private Dictionary<string, object> _resultDictionary;

        public SysAdminController(ISystemAdminService systemAdminService, ISchoolService schoolService)
        {
            _systemAdminService = systemAdminService;
            _schoolService = schoolService;
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

        [HttpGet]
        public IActionResult SchoolReg()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SchoolReg(School newSchool)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _resultDictionary = _schoolService.RegisterSchoolAsync(newSchool).Result;
                    if (!(bool)_resultDictionary["Success"]) throw new(_resultDictionary["ErrorMessage"] as string);
                    return RedirectToAction(nameof(SysAdminLandingPage));
                }
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n" + ex.Message.ToUpper() + "\n\n");
                return RedirectToAction("Index, Home");
            }
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

                var admin = _resultDictionary["Result"] as SysAdmin;

                BiModelHelper model = new()
                {
                    Admin = admin!,
                    EmisNumber = admin!.SysAdminSchoolNP!.EmisNumber
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
