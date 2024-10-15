using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
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
        private readonly ILearnerService _learnerService;
        private Dictionary<string, object> _returnDictionary;
        private static readonly string[] separator = new[] { ",", ", " };

        public SysAdminController(ISystemAdminService systemAdminService, ISchoolService schoolService, ITeacherService teacherService, IPrincipalService principalService, ILearnerService learnerService)
        {
            _systemAdminService = systemAdminService;
            _schoolService = schoolService;
            _teacherService = teacherService;
            _principalService = principalService;
            _learnerService = learnerService;
            _returnDictionary = [];
        }

        [Authorize(Roles = "System Admin")]
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
                    Learner = new()
                    {
                        Parents =
                        [
                            new()
                        ]
                    },
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

                IFormFile? file = Request.Form.Files.FirstOrDefault();
                if (ModelState.IsValid)
                {
                    string actor = "";

                    if (model.Principal is not null)
                    {
                        actor = "Principal";
                        model.Principal.ProfileImage = file?.FileName;
                        _returnDictionary = _schoolService.GetSchoolByIdAsync(model.Principal.SchoolID).Result;
                    }
                    else if (model.Teacher is not null)
                    {
                        actor = "Teacher";
                        model.Teacher.ProfileImage = file?.FileName;
                        _returnDictionary = _schoolService.GetSchoolByIdAsync(model.Teacher.SchoolID).Result;
                    }
                    else if (model.Parent is not null)
                    {
                        actor = "Parent";
                        model.Parent.ProfileImage = file?.FileName;
                        long? schoolId = model.Parent.Children.FirstOrDefault().Learner.SchoolID;
                        if (schoolId is null)
                            throw new("Something went wrong, some values may have been null.");

                        _returnDictionary = _schoolService.GetSchoolByIdAsync((long)schoolId).Result;
                    }
                    else if (model.Learner is not null)
                    {
                        model.Learner.ProfileImage = file?.FileName;
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
                    else if (actor == "Teacher")
                    {
                        if (model.Teacher.Subjects.First().Contains(','))
                            model.Teacher.Subjects.RemoveAt(0);
                        model.Teacher.Subjects = model.Teacher.Subjects.Distinct() as List<string>;


                        _returnDictionary = _teacherService.RegisterTeacherAsync(model.Teacher!).Result;

                        if (!(bool)_returnDictionary["Success"])
                            throw new(_returnDictionary["ErrorMessage"] as string);
                    }
                    else if (actor == "Learner")
                    {
                        model.Learner!.Subjects!.RemoveAt(0);
                        foreach (var item in model.Learner.Parents)
                        {
                            item.LearnerIdNo = model.Learner.IdNo;
                            item.Parent.IdNo = item.ParentIdNo;
                        }

                        _returnDictionary = _learnerService.RegisterLearnerAsync(model.Learner!).Result;
                        if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
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

                model.Learner = new()
                {
                    Parents =
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

                model.Learner = new()
                {
                    Parents =
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

        public IActionResult SysAdminUpdateDetails()
        {
            return View();
        }
    }
}
