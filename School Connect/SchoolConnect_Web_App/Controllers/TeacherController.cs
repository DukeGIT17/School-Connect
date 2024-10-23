using Microsoft.AspNetCore.Mvc;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_Web_App.IServices;
using SchoolConnect_Web_App.Models;


namespace schoolconnect.Controllers
{
    public class TeacherController : Controller
    {
        private readonly ITeacherService _teacherService;
        private readonly IAnnouncementService _announcementService;
        private Dictionary<string, object> _returnDictionary;
        public TeacherController(ITeacherService teacherService, IAnnouncementService announcementService)
        {
            _teacherService = teacherService;
            _announcementService = announcementService;
            _returnDictionary = [];
        }

        [HttpGet]
        public IActionResult TeacherLandingPage(long id)
        {
            try
            {
                _returnDictionary = _teacherService.GetTeacherByIdAsync(id).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                return View(_returnDictionary["Result"]);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n" + ex.Message.ToUpper() + "\n\n");
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult TeacherViewProfile(long id)
        {
            try
            {
                _returnDictionary = _teacherService.GetTeacherByIdAsync(id).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                return View(_returnDictionary["Result"]);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n" + ex.Message.ToUpper() + "\n\n");
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult TeacherMakeAnnouncements(long id)
        {
            try
            {
                _returnDictionary = _teacherService.GetTeacherByIdAsync(id).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                if (_returnDictionary["Result"] is not Teacher teacher) throw new("Something went wrong, could not acquire teacher data.");
                ActorAnnouncementViewModel<Teacher> model = new()
                {
                    Actor = teacher,
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
        public IActionResult TeacherMakeAnnouncement(ActorAnnouncementViewModel<Teacher> model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _returnDictionary = _announcementService.CreateAnnouncementAsync(model.Announcement).Result;
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                    return RedirectToAction("TeacherLandingPage", new { id = model.Announcement.TeacherID });
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
        public IActionResult TeacherViewAnnouncements(long id)
        {
            try
            {
                _returnDictionary = _announcementService.GetAnnouncementByTeacherId(id).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                return View(_returnDictionary["Result"]);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n" + ex.Message.ToUpper() + "\n\n");
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult TeacherDetailedAnnouncement()
        {
            return View();
        }

        public IActionResult TeacherClassRoaster()
        {
            return View();
        }

        public IActionResult TeacherMarkAttendance()
        {
            return View();
        }

        public IActionResult TeacherMakeReports()
        {
            return View();
        }

        public IActionResult TeacherViewLearnerProfile()
        {
            return View();
        }

        public IActionResult TeacherViewGrades()
        {
            return View();
        }

        public IActionResult TeacherSubjectPage()
        {
            return View();
        }

        public IActionResult TeacherChatList()
        {
            return View();
        }

        [HttpGet]
        public IActionResult TeacherUpdateDetails(long id)
        {
            try
            {
                _returnDictionary = _teacherService.GetTeacherByIdAsync(id).Result;
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
        public IActionResult TeacherUpdateDetails(Teacher teacher)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _returnDictionary = _teacherService.UpdateTeacherAsync(teacher).Result;
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                    return RedirectToAction("TeacherViewProfile", new { id = teacher.Id });
                }
                return View(teacher);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n" + ex.Message.ToUpper() + "\n\n");
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
