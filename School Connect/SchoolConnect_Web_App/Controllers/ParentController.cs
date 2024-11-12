using Microsoft.AspNetCore.Mvc;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_Web_App.IServices;
using SchoolConnect_Web_App.Models;

namespace SchoolConnect_Web_App.Controllers
{
    public class ParentController : Controller
    {
        private readonly IParentService _parentService;
        private readonly ISchoolService _schoolService;
        private readonly ILearnerService _learnerService;
        private readonly IAnnouncementService _announcementService;
        private Dictionary<string, object> _returnDictionary;

        public ParentController(IParentService parentService, ISchoolService schoolService, ILearnerService learnerService, IAnnouncementService announcementService)
        {
            _parentService = parentService;
            _schoolService = schoolService;
            _learnerService = learnerService;
            _announcementService = announcementService;
            _returnDictionary = [];
        }

        [HttpGet]
        public IActionResult ParentLandingPage(long id)
        {
            try
            {
                _returnDictionary = _parentService.GetParentByIdAsync(id).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                if (_returnDictionary["Result"] is not Parent parent) throw new("Could not acquire parent data from the provided dictionary");
                return View(parent);
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Home", new { errorMessage = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult ParentViewProfile(long parentId)
        {
            try
            {
                _returnDictionary = _parentService.GetParentByIdAsync(parentId).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                if (_returnDictionary["Result"] is not Parent parent) throw new("Could not acquire parent data from the provided dictionary");
                return View(parent);
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Home", new { errorMessage = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult ParentUpdateDetails(long parentId)
        {
            try
            {
                _returnDictionary = _parentService.GetParentByIdAsync(parentId).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                if (_returnDictionary["Result"] is not Parent parent) throw new("Could not acquire parent data from the provided dictionary");
                return View(parent);
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Home", new { errorMessage = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult ParentUpdateDetails(Parent parent)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _returnDictionary = _parentService.UpdateParentInfo(parent).Result;
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                    return RedirectToAction(nameof(ParentViewProfile), new { parentId = parent.Id });
                }
                return View(parent);
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Home", new { errorMessage = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult ParentSchoolProfile(long parentId, long schoolId)
        {
            try
            {
                _returnDictionary = _schoolService.GetSchoolAndLearnersAsync(parentId, schoolId).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                if (_returnDictionary["Result"] is not School school) throw new("Could not acquire school data from the provided dictionary");
                school.SchoolLogoBase64 = $"data:image/{school.SchoolLogoType};base64,{school.SchoolLogoBase64}";

                _returnDictionary = _parentService.GetParentByIdAsync(parentId).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                if (_returnDictionary["Result"] is not Parent parent) throw new("Could not acquire parent data from the provided dictionary.");

                return View(new ParentModelViewModel<School>
                {
                    ActualModel = school,
                    Parent = parent
                });
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Home", new { errorMessage = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult ParentViewChildProfile(long learnerId, long parentId)
        {
            try
            {
                _returnDictionary = _learnerService.GetLearnerByIdAsync(learnerId).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                if (_returnDictionary["Result"] is not Learner learner) throw new("Could not acquire learner data from the provided dictionary");
                learner.ProfileImageBase64 = $"data:image/{learner.ProfileImageType};base64,{learner.ProfileImageBase64}";

                _returnDictionary = _parentService.GetParentByIdAsync(parentId).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                if (_returnDictionary["Result"] is not Parent parent) throw new("Could not acquire parent data from the provided dictionary");

                return View(new ParentModelViewModel<Learner>
                {
                    ActualModel = learner,
                    Parent = parent
                });
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Home", new { errorMessage = ex.Message });
            }
        }

        public IActionResult ParentViewAnnouncements(long parentId)
        {
            try
            {
                _returnDictionary = _parentService.GetParentByIdAsync(parentId).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                if (_returnDictionary["Result"] is not Parent parent) throw new("Could not acquire parent data from the provided dictionary.");

                _returnDictionary = _announcementService.GetAnnouncementsByParentIdAsync(parentId).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                if (_returnDictionary["Result"] is not IEnumerable<Announcement> announcements) throw new("Could not acquire announcements from the provided dictionary.");

                List<ActorAnnouncementViewModel<Parent>> model = [];
                foreach (var ann in announcements)
                {
                    model.Add(new ActorAnnouncementViewModel<Parent>
                    {
                        Actor = parent,
                        Announcement = ann
                    });
                }
                return View(model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Home", new { errorMessage = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult ParentDetailedAnnouncement(int id, string parentIdNo)
        {
            try
            {
                _returnDictionary = _announcementService.GetAnnouncementById(id).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                if (_returnDictionary["Result"] is not Announcement ann) throw new("Could not acquire announcement data.");

                if (!ann.ViewedRecipients!.Contains(parentIdNo) && parentIdNo is not null)
                {
                    ann.ViewedRecipients!.Add(parentIdNo);
                    _returnDictionary = _announcementService.UpdateAnnouncementAsync(ann).Result;
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                }

                return View(ann);
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Home", new { errorMessage = ex.Message });
            }
        }

        public IActionResult ParentViewAttendance()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ParentChatList(long parentId)
        {
            try
            {
                _returnDictionary = _parentService.GetTeachersByParentAync(parentId).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                if (_returnDictionary["Result"] is not IEnumerable<Teacher> teachers) throw new("Could not acquire teacher data from the provided dictionary.");

                foreach (var teacher in teachers)
                    teacher.ProfileImageBase64 = $"data:image/{teacher.ProfileImageType};base64,{teacher.ProfileImageBase64}";

                _returnDictionary = _parentService.GetParentByIdAsync(parentId).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                if (_returnDictionary["Result"] is not Parent parent) throw new("Could not acquire parent data from the provided dictionary");
                
                return View(new ParentChatViewModel
                {
                    Teachers = teachers,
                    Parent = parent
                });
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Home", new { errorMessage = ex.Message });
            }
        }

        public IActionResult ParentUpdateDetails()
        {
            return View();
        }


    }
}
