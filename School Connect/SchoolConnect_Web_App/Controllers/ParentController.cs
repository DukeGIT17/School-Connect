using Microsoft.AspNetCore.Mvc;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_RepositoryLayer.Interfaces;
using SchoolConnect_Web_App.IServices;

namespace SchoolConnect_Web_App.Controllers
{
    public class ParentController : Controller
    {
        private readonly IParentService _parentService;
        private readonly ISchoolService _schoolService;
        private readonly ILearnerService _learnerService;
        private Dictionary<string, object> _returnDictionary;

        public ParentController(IParentService parentService, ISchoolService schoolService, ILearnerService learnerService)
        {
            _parentService = parentService;
            _schoolService = schoolService;
            _learnerService = learnerService;
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
                Console.WriteLine("\n\n" + ex.Message.ToUpper() + "\n\n");
                return RedirectToAction("Index", "Home");
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
                Console.WriteLine("\n\n" + ex.Message.ToUpper() + "\n\n");
                return RedirectToAction("Index", "Home");
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
                Console.WriteLine("\n\n" + ex.Message.ToUpper() + "\n\n");
                return RedirectToAction("Index", "Home");
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
                Console.WriteLine("\n\n" + ex.Message.ToUpper() + "\n\n");
                return RedirectToAction("Index", "Home");
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
                return View(school);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n" + ex.Message.ToUpper() + "\n\n");
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult ParentViewChildProfile(long learnerId)
        {
            try
            {
                _returnDictionary = _learnerService.GetLearnerByIdAsync(learnerId).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                if (_returnDictionary["Result"] is not Learner learner) throw new("Could not acquire learner data from the provided dictionary");
                learner.ProfileImageBase64 = $"data:image/{learner.ProfileImageType};base64,{learner.ProfileImageBase64}";
                return View(learner);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n" + ex.Message.ToUpper() + "\n\n");
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult ParentViewAnnouncements()
        {
            return View();
        }

        public IActionResult ParentDetailedAnnouncement()
        {
            return View();
        }

        public IActionResult ParentViewAttendance()
        {
            return View();
        }

        public IActionResult ParentViewReports()
        {
            return View();
        }


        public IActionResult ParentChatList()
        {
            return View();
        }

        public IActionResult ParentUpdateDetails()
        {
            return View();
        }


    }
}
