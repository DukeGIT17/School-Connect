using Microsoft.AspNetCore.Mvc;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_RepositoryLayer.Interfaces;
using SchoolConnect_Web_App.IServices;
using SchoolConnect_Web_App.Models;

namespace SchoolConnect_Web_App.Controllers
{
    public class SysAdminController : Controller
    {
        private readonly ISystemAdminService _systemAdminService;
        private readonly ISchoolService _schoolService;
        private readonly ITeacherService _teacherService;
        private readonly IPrincipalService _principalService;
        private Dictionary<string, object> _returnDictionary;

        public SysAdminController(ISystemAdminService systemAdminService, ISchoolService schoolService, ITeacherService teacherService, IPrincipalService principalService)
        {
            _systemAdminService = systemAdminService;
            _schoolService = schoolService;
            _teacherService = teacherService;
            _principalService = principalService;
            _returnDictionary = [];
        }

        [HttpGet]
        public IActionResult SysAdminLandingPage(long id)
        {
            _returnDictionary.Clear();
            try
            {
                _returnDictionary = _systemAdminService.GetAdminById(id);
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                return View(_returnDictionary["Result"] as SysAdmin);
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
            _returnDictionary.Clear();
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
        public IActionResult SchoolReg(ActorSchoolViewModel<SysAdmin> model)
        {
            _returnDictionary.Clear();
            try
            {
                if (ModelState.IsValid)
                {
                    _returnDictionary = _schoolService.RegisterSchoolAsync(model.School!).Result;
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                    return RedirectToAction(nameof(SysAdminLandingPage), new { id = model.School!.SystemAdminId });
                }
                return View(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n" + ex.Message.ToUpper() + "\n\n");
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpGet]
        public IActionResult RolesReg(long id)
        {
            try
            {
                _returnDictionary = _schoolService.GetSchoolByAdminAsync(id).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                var school = _returnDictionary["Result"] as School;
                ActorRegistrationViewModel model = new()
                {
                    Principal = new(),
                    Teacher = new(),
                    Parent = new()
                    {
                        Children =
                        [
                            new()
                        ]
                    },
                    Learner = new(),
                    SchoolID = school!.Id,
                    AdminID = id
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
        public IActionResult RolesReg(ActorRegistrationViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string actor = "";

                    if (model.Principal is not null)
                    {
                        actor = "Principal";
                        _returnDictionary = _schoolService.GetSchoolByIdAsync(model.Principal.SchoolID).Result;
                    }
                    else if (model.Teacher is not null)
                    {
                        actor = "Teacher";
                        _returnDictionary = _schoolService.GetSchoolByIdAsync(model.Teacher.SchoolID).Result;
                    }
                    else if (model.Parent is not null)
                    {
                        actor = "Parent";
                        long? schoolId = model.Parent.Children.FirstOrDefault().Learner.SchoolID;
                        if (schoolId is null)
                            throw new("Something went wrong, some values may have been null.");

                        _returnDictionary = _schoolService.GetSchoolByIdAsync((long)schoolId).Result;
                    }
                    else if (model.Learner is not null)
                    {
                        actor = "Learner";
                        _returnDictionary = _schoolService.GetSchoolByIdAsync(model.Learner.SchoolID).Result;
                    }
                    else
                        throw new("Something went wrong. Please review your information");

                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                    _returnDictionary.Clear();
                    if (actor == "Principal")
                    {
                        _returnDictionary = _principalService.RegisterPrincipalAsync(model.Principal!).Result;
                        if (!(bool)_returnDictionary["Success"])
                            throw new(_returnDictionary["ErrorMessage"] as string);

                    }
                    if (actor == "Teacher")
                    {
                        List<string> subjectList = [];
                        List<string> tempList = [];

                        foreach (var item in model.Teacher!.Subjects)
                            subjectList.AddRange(item.Split(", "));

                        foreach (var item in subjectList)
                            if (item.Contains(','))
                                tempList.AddRange(item.Split(","));

                        subjectList = subjectList.Distinct().ToList();
                        subjectList.Remove("");
                        model.Teacher.Subjects = subjectList;

                        _returnDictionary = _teacherService.RegisterTeacherAsync(model.Teacher!).Result;

                        if (!(bool)_returnDictionary["Success"])
                            throw new(_returnDictionary["ErrorMessage"] as string);
                    }

                    return RedirectToAction(nameof(SysAdminLandingPage), new { id = model.AdminID });
                }
                
                model.Parent = new()
                {
                    Children =
                    [
                        new()
                    ]
                };
                return View(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n" + ex.Message.ToUpper() + "\n\n");
                ModelState.AddModelError("", ex.Message);
                model.Parent = new()
                {
                    Children =
                    [
                        new()
                    ]
                };
                return View(model); 
            }
        }

        [HttpGet]
        public IActionResult SysAdminViewProfile(long id)
        {
            _returnDictionary.Clear();
            try
            {
                _returnDictionary = _systemAdminService.GetAdminById(id);
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                var admin = _returnDictionary["Result"] as SysAdmin;

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
