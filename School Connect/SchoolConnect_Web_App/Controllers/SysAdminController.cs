using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SchoolConnect_DomainLayer.Models;
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
        private readonly IParentService _parentService;
        private Dictionary<string, object> _returnDictionary;

        public SysAdminController(ISystemAdminService systemAdminService, ISchoolService schoolService, ITeacherService teacherService, IPrincipalService principalService, ILearnerService learnerService, IParentService parentService)
        {
            _systemAdminService = systemAdminService;
            _schoolService = schoolService;
            _teacherService = teacherService;
            _principalService = principalService;
            _learnerService = learnerService;
            _parentService = parentService;
            _returnDictionary = [];
        }

        [Authorize(Roles = "System Admin")]
        [HttpGet]
        public IActionResult SysAdminLandingPage(long id)
        {
            _returnDictionary.Clear();
            try
            {
                _returnDictionary = _systemAdminService.GetAdminByIdAsync(id).Result;
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
                    model.School.Logo = model.School.SchoolLogoFile?.FileName;
                    _returnDictionary = _schoolService.RegisterSchoolAsync(model.School!).Result;
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                    if (model.School.Type == "High" || model.School.Type == "Combined")
                        return RedirectToAction("GetSubjectsPage", new { adminId = model.School.SystemAdminId, newClasses = "10A, 11A, 12A" });

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
        public IActionResult GetSubjectsPage(long adminId, string? destination = null, string? newClasses = null)
        {
            try
            {
                _returnDictionary = _schoolService.GetSchoolByAdminAsync(adminId).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                if (_returnDictionary["Result"] is not School school) throw new("Could not acquire school data from the provided dictionary");

                return View("AddSubjectsPage", new SubjectsViewModel
                {
                    SchoolId = school.Id,
                    AdminId = adminId,
                    Subjects =
                    [
                        "",
                        "",
                        "",
                        "",
                        "",
                    ],
                    Destination = destination,
                    NewClasses = newClasses
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n" + ex.Message.ToUpper() + "\n\n");
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult AddSubjectsPage(SubjectsViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _returnDictionary = _schoolService.AddSubjectsAsync(model.Subjects, model.SchoolId, model.NewClasses).Result;
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                    if (model.Destination is not null)
                        return RedirectToAction(model.Destination.Split(',').First(), model.Destination.Split(',').Last(), new { schoolId = model.SchoolId });

                    return RedirectToAction(nameof(SysAdminLandingPage), new { id = model.AdminId });
                }

                _returnDictionary = _schoolService.GetSchoolByAdminAsync(model.AdminId).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                if (_returnDictionary["Result"] is not School school) throw new("Could not acquire school data from the provided dictionary");

                return View(new SubjectsViewModel
                {
                    SchoolId = school.Id,
                    AdminId = model.AdminId,
                    Subjects =
                    [
                        "",
                        "",
                        "",
                        "",
                        "",
                    ],
                    Destination = model.Destination,
                    NewClasses = model.NewClasses,
                });
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
                if (_returnDictionary["Result"] is not School school) throw new("Could not acquire school data from the provided dictionary.");
                ActorRegistrationViewModel model = new()
                {
                    Principal = school.SchoolPrincipalNP,
                    Teacher = new(),
                    Parent = new()
                    {
                        Children = [new()]
                    },
                    Learner = new()
                    {
                        Parents = [new()]
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
                if (ModelState.IsValid)
                {
                    if (model.BulkLoadFile is not null)
                    {
                        _returnDictionary = model.Actor switch
                        {
                            "Learner" => _learnerService.BulkLoadLearnersAsync(model.BulkLoadFile, model.SchoolID).Result,
                            "Parent" => _parentService.BulkLoadParentsAsync(model.BulkLoadFile).Result,
                            "Teacher" => _teacherService.BulkLoadTeachersAsync(model.BulkLoadFile, model.SchoolID).Result,
                            _ => throw new("Something went wrong. Which actor are you bulk loading?! >:("),
                        };
                    }
                    else if (model.Principal is not null)
                    {
                        _returnDictionary = _principalService.RegisterPrincipalAsync(model.Principal!).Result;
                        if (!(bool)_returnDictionary["Success"])
                            throw new(_returnDictionary["ErrorMessage"] as string);
                    }
                    else if (model.Teacher is not null)
                    {
                        int index = model.Teacher!.Subjects.IndexOf(model.Teacher.Subjects.FirstOrDefault(s => s.Contains(',')) ?? "");
                        if (index > -1)
                            model.Teacher.Subjects.RemoveAt(index);
                        model.Teacher.Subjects = model.Teacher.Subjects.Distinct().ToList();

                        _returnDictionary = _teacherService.RegisterTeacherAsync(model.Teacher!).Result;
                    }
                    else if (model.Parent is not null)
                    {
                        model.Parent.Children.First().ParentIdNo = model.Parent.IdNo;
                        _returnDictionary = _parentService.RegisterParentAsync(model.Parent).Result;
                    }
                    else if (model.Learner is not null)
                        _returnDictionary = _learnerService.RegisterLearnerAsync(model.Learner!).Result;
                    else
                        throw new("Something went wrong. Please review your information");

                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                    return RedirectToAction(nameof(SysAdminLandingPage), new { id = model.AdminID });
                }

                model.Parent = new()
                {
                    Children = [new()]
                };

                model.Learner = new()
                {
                    Parents = [new()]
                };
                return View(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n" + ex.Message.ToUpper() + "\n\n");
                ModelState.AddModelError("", ex.Message);
                model.Parent = new()
                {
                    Children = [new()]
                };

                model.Learner = new()
                {
                    Parents = [new()]
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
                _returnDictionary = _systemAdminService.GetAdminByIdAsync(id).Result;
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

        [HttpGet]
        public IActionResult SysAdminUpdateDetails(long id)
        {
            try
            {
                _returnDictionary = _systemAdminService.GetAdminByIdAsync(id).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                return View(_returnDictionary["Result"]);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n" + ex.Message.ToUpper() + "\n\n");
                return RedirectToAction("Index", "Home");
            }
        }
        
        [HttpPost]
        public IActionResult SysAdminUpdateDetails(SysAdmin admin)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _returnDictionary = _systemAdminService.UpdateAsync(admin).Result;
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                    return RedirectToAction("SysAdminViewProfile", new { id = admin.Id });
                }
                return View(admin);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n" + ex.Message.ToUpper() + "\n\n");
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
