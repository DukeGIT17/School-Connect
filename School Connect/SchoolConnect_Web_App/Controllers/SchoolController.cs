using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_Web_App.IServices;
using SchoolConnect_Web_App.Models;

namespace SchoolConnect_Web_App.Controllers
{
    public class SchoolController : Controller
    {
        private readonly ISchoolService _schoolService;
        private readonly ITeacherService _teacherService;
        private Dictionary<string, object> _returnDictionary;

        public SchoolController(ISchoolService schoolService, ITeacherService teacherService)
        {
            _schoolService = schoolService;
            _teacherService = teacherService;
            _returnDictionary = [];
        }

        [HttpGet]
        public async Task<IActionResult> ListOfSchools()
        {
            try
            {
                _returnDictionary = await _schoolService.GetSchools();
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                if (_returnDictionary["Result"] is not IEnumerable<School> schools) throw new("Could not extract a collection of schools from the provided dictionary.");
                return View(schools);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n" + ex.Message.ToUpper() + "\n\n");
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult SchoolManagementLandingPage(long schoolId)
        {
            try
            {
                _returnDictionary = _schoolService.GetSchoolByIdAsync(schoolId).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                if (_returnDictionary["Result"] is not School school) throw new("Could not extract school data from the provided dictionary.");
                return View(school);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n" + ex.Message.ToUpper() + "\n\n");
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult ViewSchoolProfile(long schoolId)
        {
            try
            {
                _returnDictionary = _schoolService.GetSchoolByIdAsync(schoolId).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                if (_returnDictionary["Result"] is not School school) throw new("Could not extract school data from the provided dictionary.");
                return View(school);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n" + ex.Message.ToUpper() + "\n\n");
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult SchoolUpdateDetails(long schoolId)
        {
            try
            {
                _returnDictionary = _schoolService.GetSchoolByIdAsync(schoolId).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                if (_returnDictionary["Result"] is not School school) throw new("Could not extract school data from the provided dictionary.");
                return View(school);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n" + ex.Message.ToUpper() + "\n\n");
                return RedirectToAction("Index", "Home");
            }
        }
        
        [HttpPost]
        public IActionResult SchoolUpdateDetails(School school)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _returnDictionary = _schoolService.UpdateSchoolAsync(school).Result;
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                    return RedirectToAction("ViewSchoolProfile", new { schoolId = school.Id });
                }
                return View(school);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n" + ex.Message.ToUpper() + "\n\n");
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult ManageTeacherClassAssignment(long schoolId)
        {
            try
            {
                _returnDictionary = _teacherService.GetTeachersBySchoolAsync(schoolId).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                if (_returnDictionary["Result"] is not IEnumerable<Teacher> teachers) throw new("Could not acquire teachers from the provided dictionary.");

                _returnDictionary = _schoolService.GetAllClassesBySchoolAsync(schoolId).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                if (_returnDictionary["Result"] is not IEnumerable<SubGrade> classes) throw new("Could not acquire classes from the provided dictionary.");

                TeacherClassAssignmentViewModel model = new()
                {
                    Teachers = teachers,
                    Classes = classes,
                    MainClass = false,
                    SchoolId = schoolId
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
        public IActionResult ManageTeacherClassAssignment(TeacherClassAssignmentViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _returnDictionary = _teacherService.GetTeacherByEmailAddressAsync(model.TeacherEmailAddress).Result;
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                    if (_returnDictionary["Result"] is not Teacher teacher) throw new("Could not acquire teacher data from the provided dictionary.");

                    _returnDictionary = _schoolService.GetClassBySchoolAsync(model.ClassDesignate, model.SchoolId).Result;
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                    if (_returnDictionary["Result"] is not SubGrade subGrade) throw new("Could not acquire class data from the provided dictionary.");

                    if (model.MainClass)
                    {
                        subGrade.MainTeacherId = teacher.Id;
                        teacher.MainClass = subGrade;
                    }
                    else
                    {
                        if (teacher.Classes is null)
                            teacher.Classes = [];

                        teacher.Classes.Add(new()
                        {
                            TeacherID = teacher.Id,
                            ClassID = subGrade.Id,
                            StaffNr = teacher.StaffNr,
                            ClassDesignate = subGrade.ClassDesignate,
                            Class = subGrade
                        });
                    }

                    _returnDictionary = _teacherService.UpdateClassAllocationAsync(teacher).Result;
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                    return RedirectToAction("SchoolManagementLandingPage", new { schoolId = teacher.SchoolID });
                }

                _returnDictionary = _teacherService.GetTeachersBySchoolAsync(model.SchoolId).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                if (_returnDictionary["Result"] is not IEnumerable<Teacher> teachers) throw new("Could not acquire teachers from the provided dictionary.");

                _returnDictionary = _schoolService.GetAllClassesBySchoolAsync(model.SchoolId).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                if (_returnDictionary["Result"] is not IEnumerable<SubGrade> classes) throw new("Could not acquire classes from the provided dictionary.");

                model.Teachers = teachers;
                model.Classes = classes;
                return View(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n" + ex.Message.ToUpper() + "\n\n");
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult ManageSchoolGrades(long schoolId)
        {
            try
            {
                _returnDictionary = _schoolService.GetAllClassesBySchoolAsync(schoolId).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                if (_returnDictionary["Result"] is not IEnumerable<SubGrade> classes) throw new("Could not acquire class from the provided dictionary.");

                classes = [.. classes.OrderBy(c => c.ClassDesignate.StartsWith('R') ? 0 : 1).ThenBy(c => c.ClassDesignate.StartsWith('R') ? 0 : Convert.ToInt32(c.ClassDesignate.Remove(c.ClassDesignate.IndexOf(c.ClassDesignate.Last()))))];
                GradeClassesViewModel model = new()
                {
                    Classes = classes,
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
        public IActionResult ManageSchoolGrades(GradeClassesViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    List<string> classDesignates = model.ClassesToAdd.Split([',', ' '], StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
                    _returnDictionary = _schoolService.AddClassesToSchool(classDesignates, model.SchoolId).Result;
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                    if (!classDesignates.Select(s => s.Remove(s.IndexOf(s.Last()))).Intersect(["10", "11", "12"]).IsNullOrEmpty())
                        return RedirectToAction("GetSubjectsPage", "SysAdmin", new { adminId = model.SchoolId, destination = "SchoolManagementLandingPage,School", newClasses = model.ClassesToAdd });

                    return RedirectToAction("SchoolManagementLandingPage", new { schoolId = model.SchoolId });
                }

                _returnDictionary = _schoolService.GetAllClassesBySchoolAsync(model.SchoolId).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                if (_returnDictionary["Result"] is not IEnumerable<SubGrade> classes) throw new("Could not acqurie classes from the provided dictionary.");
                model.Classes = classes;
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
