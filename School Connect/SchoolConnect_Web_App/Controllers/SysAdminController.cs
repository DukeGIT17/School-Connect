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
        private Dictionary<string, object> _resultDictionary;

        public SysAdminController(ISystemAdminService systemAdminService, ISchoolService schoolService, ITeacherService teacherService)
        {
            _systemAdminService = systemAdminService;
            _schoolService = schoolService;
            _teacherService = teacherService;
            _resultDictionary = [];
        }

        [HttpGet]
        public IActionResult SysAdminLandingPage(long id)
        {
            _resultDictionary.Clear();
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
            _resultDictionary.Clear();
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
            _resultDictionary.Clear();
            try
            {
                if (ModelState.IsValid)
                {
                    _resultDictionary = _schoolService.RegisterSchoolAsync(model.School!).Result;
                    if (!(bool)_resultDictionary["Success"]) throw new(_resultDictionary["ErrorMessage"] as string);
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
                _resultDictionary = _schoolService.GetSchoolByAdminAsync(id).Result;
                if (!(bool)_resultDictionary["Success"]) throw new(_resultDictionary["ErrorMessage"] as string);
                var school = _resultDictionary["Result"] as School;
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
                        _resultDictionary = _schoolService.GetSchoolByIdAsync(model.Principal.SchoolID).Result;
                    }
                    else if (model.Teacher is not null)
                    {
                        actor = "Teacher";
                        _resultDictionary = _schoolService.GetSchoolByIdAsync(model.Teacher.SchoolID).Result;
                    }
                    else if (model.Parent is not null)
                    {
                        actor = "Parent";
                        long? schoolId = model.Parent.Children.FirstOrDefault().Learner.SchoolID;
                        if (schoolId is null)
                            throw new("Something went wrong, some values may have been null.");

                        _resultDictionary = _schoolService.GetSchoolByIdAsync((long)schoolId).Result;
                    }
                    else if (model.Learner is not null)
                    {
                        actor = "Learner";
                        _resultDictionary = _schoolService.GetSchoolByIdAsync(model.Learner.SchoolID).Result;
                    }
                    else
                        throw new("Something went wrong. Please review your information");

                    if (!(bool)_resultDictionary["Success"]) throw new(_resultDictionary["ErrorMessage"] as string);
                    if (actor == "Teacher")
                    {
                        _resultDictionary.Clear();

                        List<string> subjectList = [];
                        foreach (var item in model.Teacher!.Subjects)
                            subjectList.AddRange(item.Split(", "));
                        subjectList = subjectList.Distinct().ToList();
                        subjectList.Remove("");
                        model.Teacher.Subjects = subjectList;
                        _resultDictionary = _teacherService.RegisterTeacherAsync(model.Teacher!).Result;

                        if (!(bool)_resultDictionary["Success"]) 
                            throw new(_resultDictionary["ErrorMessage"] as string);
                    }

                    return RedirectToAction(nameof(SysAdminLandingPage));
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
        public IActionResult SysAdminViewProfile(long id)
        {
            _resultDictionary.Clear();
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
