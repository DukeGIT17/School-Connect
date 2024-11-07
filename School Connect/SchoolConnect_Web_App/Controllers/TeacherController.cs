using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_Web_App.IServices;
using SchoolConnect_Web_App.Models;


namespace schoolconnect.Controllers
{
    public class TeacherController : Controller
    {
        private readonly ITeacherService _teacherService;
        private readonly IAnnouncementService _announcementService;
        private readonly ILearnerService _learnerService;
        private Dictionary<string, object> _returnDictionary;
        public TeacherController(ITeacherService teacherService, IAnnouncementService announcementService, ILearnerService learner)
        {
            _teacherService = teacherService;
            _announcementService = announcementService;
            _learnerService = learner;
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
                    StaffNr = teacher.StaffNr
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
                    model.Announcement.ViewedRecipients = [];
                    model.Announcement.ViewedRecipients.Add(model.StaffNr!);

                    _returnDictionary = _announcementService.CreateAnnouncementAsync(model.Announcement).Result;
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                    return RedirectToAction("TeacherLandingPage", new { id = model.Announcement.TeacherID });
                }

                _returnDictionary = _teacherService.GetTeacherByIdAsync((long)model.Announcement.TeacherID!).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                if (_returnDictionary["Result"] is not Teacher teacher) throw new("Something went wrong, could not acquire teacher data.");
                
                model.Actor = teacher;
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
                _returnDictionary = _teacherService.GetTeacherByIdAsync(id).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                if (_returnDictionary["Result"] is not Teacher teacher) throw new("Something went wrong, could not acquire teacher data from the provided dictionary.");

                _returnDictionary = _announcementService.GetAnnouncementByTeacherId(id).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                if (_returnDictionary["Result"] is not IEnumerable<Announcement> announcements) throw new("Could not acquire announcements from the provided dictionary.");

                List<ActorAnnouncementViewModel<Teacher>> annCollection = [];
                foreach (var announcement in announcements)
                {
                    annCollection.Add(new()
                    {
                        Actor = teacher,
                        Announcement = announcement,
                        StaffNr = teacher.StaffNr
                    });
                }

                return View(annCollection);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n" + ex.Message.ToUpper() + "\n\n");
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult DeleteAnnouncement(int id, long teacherId)
        {
            try
            {
                _returnDictionary = _announcementService.RemoveAnnouncementAsync(id).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                return RedirectToAction("TeacherViewAnnouncements", new { id = teacherId });
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n" + ex.Message.ToUpper() + "\n\n");
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult TeacherDetailedAnnouncement(int id, string teacherStaffNr)
        {
            try
            {
                _returnDictionary = _announcementService.GetAnnouncementById(id).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                if (_returnDictionary["Result"] is not Announcement ann) throw new("Could not acquire announcement data.");

                if (!ann.ViewedRecipients!.Contains(teacherStaffNr))
                {
                    ann.ViewedRecipients!.Add(teacherStaffNr);
                    _returnDictionary = _announcementService.UpdateAnnouncementAsync(ann).Result;
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                }

                return View(ann);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n" + ex.Message.ToUpper() + "\n\n");
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult TeacherClassRoster(long teacherId)
        {
            try
            {
                _returnDictionary = _learnerService.GetLearnersByClassAsync(teacherId).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                if (_returnDictionary["Result"] is not IEnumerable<Learner> learners) throw new("Could not acquire learners from the provided dictionary.");
                return View(learners);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n" + ex.Message.ToUpper() + "\n\n");
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult TeacherMarkAttendance(long teacherId)
        {
            try
            {
                _returnDictionary = _teacherService.GetAttendanceRecordsByTeacher(teacherId).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                if (_returnDictionary["Result"] is not Teacher teacher) throw new("Could not acquire teacher data from the provided dictionary.");

                TeacherMarkAttendanceViewModel model = new()
                {
                    Teacher = teacher,
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
        public IActionResult TeacherMarkAttendance(TeacherMarkAttendanceViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _returnDictionary = _teacherService.MarkAttendanceAsync(model.AttendanceRecords).Result;
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                    return RedirectToAction(nameof(TeacherClassRoster), new { teacherId = model.AttendanceRecords.First().TeacherId });
                }

                _returnDictionary = _teacherService.GetAttendanceRecordsByTeacher(model.AttendanceRecords.First().TeacherId).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                if (_returnDictionary["Result"] is not Teacher teacher) throw new("Could not acquire teacher data from the provided dictionary.");

                model.Teacher = teacher;
                return View(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n" + ex.Message.ToUpper() + "\n\n");
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult TeacherMakeReports()
        {
            return View();
        }

        [HttpGet]
        public IActionResult TeacherViewLearnerProfile(string learnerIdNo)
        {
            try
            {
                _returnDictionary = _learnerService.GetLearnerByIdNoAsync(learnerIdNo).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                if (_returnDictionary["Result"] is not Learner learner) throw new("Could not acquire learner data from the specified dictionary.");
                return View(learner);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n" + ex.Message.ToUpper() + "\n\n");
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult TeacherViewGrades(long teacherId)
        {
            try
            {
                _returnDictionary = _teacherService.GetGradesByTeacher(teacherId).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                if (_returnDictionary["Result"] is not IEnumerable<Grade> grades) throw new("Could not acquire grades from the provided dictionary.");
                return View(grades);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n" + ex.Message.ToUpper() + "\n\n");
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult TeacherViewGrades()
        {
            return View();
        }

        public IActionResult TeacherSubjectPage()
        {
            return View();
        }

        [HttpGet]
        public IActionResult TeacherChatList(long teacherId)
        {
            try
            {
                _returnDictionary = _teacherService.GetParentsByTeacherClassesAsync(teacherId).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                if (_returnDictionary["Result"] is not IEnumerable<Parent> parents) throw new("Could not acquire parents from the provided dictionary.");

                parents.ToList().ForEach(parent =>
                {
                    var cleanBase64 = parent.ProfileImageBase64!.Contains(',') ? parent.ProfileImageBase64.Split(',')[1] : parent.ProfileImageBase64;
                    var stream = new MemoryStream(Convert.FromBase64String(cleanBase64));

                    IFormFile file = new FormFile(stream, 0, stream.Length, "ProfileImage", "ProfileImage")
                    {
                        Headers = new HeaderDictionary(),
                        ContentType = "image/" + parent.ProfileImageType!
                    };

                    parent.ProfileImageFile = file;

                    parent.ProfileImageBase64 = $"data:image/{parent.ProfileImageType};base64,{parent.ProfileImageBase64}";
                });

                _returnDictionary = _teacherService.GetTeacherByIdAsync(teacherId).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                if (_returnDictionary["Result"] is not Teacher teacher) throw new("Could not acquire teacher data from the provided dictionary.");

                TeacherChatViewModel model = new()
                {
                    Parents = parents,
                    Teacher = teacher
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
