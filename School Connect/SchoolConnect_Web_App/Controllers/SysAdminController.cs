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
        public IActionResult SchoolReg(long id)
        {
            try
            {
                ActorSchoolViewModel<SysAdmin> model = new()
                {
                    Actor = new()
                    {
                        Id = id
                    },
                    School = new()
                    {
                        SchoolAddress = new()
                    }
                };

                return View(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n" + ex.Message.ToUpper() + "\n\n");
                return RedirectToAction("Index", "Home");
            }
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
                ActorSchoolViewModel<SysAdmin> model = new()
                {
                    Actor = new()
                    {
                        Id = newSchool.Id
                    },
                    School = newSchool
                };
                return View(newSchool);
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

                ActorSchoolViewModel<SysAdmin> model = new()
                {
                    Actor = admin!,
                    School = admin!.SysAdminSchoolNP
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
