using Microsoft.AspNetCore.Mvc;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_Web_App.IServices;
using SchoolConnect_Web_App.Models;

namespace SchoolConnect_Web_App.Controllers
{
    public class PrincipalController : Controller
    {
        private readonly IPrincipalService _principalService;
        private readonly IAnnouncementService _announcementService;
        private readonly ISchoolService _schoolService;
        private Dictionary<string, object> _returnDictionary;

        public PrincipalController(IPrincipalService principalService, IAnnouncementService announcementService, ISchoolService schoolService)
        {
            _principalService = principalService;
            _announcementService = announcementService;
            _schoolService = schoolService;
            _returnDictionary = [];
        }

        [HttpGet]
        public IActionResult PrincipalLandingPage(long id)
        {
            _returnDictionary.Clear();
            try
            {
                _returnDictionary = _principalService.GetPrincipalByIdAsync(id).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                return View(_returnDictionary["Result"] as Principal);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n" + ex.Message.ToUpper() + "\n\n");
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult PrincipalViewProfile(long id)
        {
            _returnDictionary.Clear();
            try
            {
                _returnDictionary = _principalService.GetPrincipalByIdAsync(id).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                return View(_returnDictionary["Result"] as Principal);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n" + ex.Message.ToUpper() + "\n\n");
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult PrincipalMakeAnnouncements(long id)
        {
            try
            {
                _returnDictionary = _principalService.GetPrincipalByIdAsync(id).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                if (_returnDictionary["Result"] is not Principal principal) throw new("Could not acquire principal data from the provided dictionary.");
                ActorAnnouncementViewModel<Principal> model = new()
                {
                    Actor = principal,
                    StaffNr = principal.StaffNr
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
        public IActionResult PrincipalMakeAnnouncements(ActorAnnouncementViewModel<Principal> model)
        {
            _returnDictionary.Clear();
            try
            {
                if (ModelState.IsValid)
                {
                    model.Announcement.ViewedRecipients = [];
                    model.Announcement.ViewedRecipients.Add(model.StaffNr!);

                    _returnDictionary = _announcementService.CreateAnnouncementAsync(model.Announcement).Result;
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                    return RedirectToAction(nameof(PrincipalLandingPage), new { id = model.Announcement.PrincipalID });
                }

                _returnDictionary = _principalService.GetPrincipalByIdAsync((long)model.Announcement.PrincipalID!).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                if (_returnDictionary["Result"] is not Principal principal) throw new("There are validation errors!! Could not acquire principal data from the provided dictionary.");
                
                model.Actor = principal;
                return View(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n" + ex.Message.ToUpper() + "\n\n");
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult PrincipalViewAnnouncements(long id)
        {
            try
            {
                _returnDictionary = _principalService.GetPrincipalByIdAsync(id).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                if (_returnDictionary["Result"] is not Principal principal) throw new("Something went wrong, could not acquire principal data from the provided dictionary.");

                _returnDictionary = _announcementService.GetAllAnnBySchoolAsync(principal.SchoolID).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                if (_returnDictionary["Result"] is not IEnumerable<Announcement> announcements) throw new("Something went wrong, could not acquire announcements from the provided dictionary.");

                List<ActorAnnouncementViewModel<Principal>> annCollection = [];
                foreach (var announcment in announcements)
                {
                    annCollection.Add(new()
                    {
                        Actor = principal,
                        Announcement = announcment,
                        StaffNr = principal.StaffNr
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
        public IActionResult PrincipalViewGrades(long schoolId)
        {
            try
            {
                _returnDictionary = _schoolService.GetGradesBySchoolAsync(schoolId).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                if (_returnDictionary["Result"] is not IEnumerable<Grade> grades) throw new("Could not acquire grades data from the provided dictionary.");

                return View(grades);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n" + ex.Message.ToUpper() + "\n\n");
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult PrincipalGradeLandingPage(long teacherId)
        {
            try
            {
                //_returnDictionary = _schoolService.GetClassBySchoolAsync()
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n" + ex.Message.ToUpper() + "\n\n");
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult PrincipalViewAttendance()
        {
            return View();
        }

        public IActionResult PrincipalLearnerProfile()
        {
            return View();
        }

        public IActionResult PrincipalViewReports()
        {
            return View();
        }

        [HttpGet]
        public IActionResult PrincipalDetailedAnnouncement(int id, string principalStaffNr)
        {
            try
            {
                _returnDictionary = _announcementService.GetAnnouncementById(id).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                if (_returnDictionary["Result"] is not Announcement ann) throw new("Could not acquire announcement data.");

                if (!ann.ViewedRecipients!.Contains(principalStaffNr))
                {
                    ann.ViewedRecipients!.Add(principalStaffNr);
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
        public IActionResult PrincipalUpdateDetails(long id)
        {
            try
            {
                _returnDictionary = _principalService.GetPrincipalByIdAsync(id).Result;
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
        public IActionResult DeleteAnnouncement(int id, long principalId)
        {
            try
            {
                _returnDictionary = _announcementService.RemoveAnnouncementAsync(id).Result;
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                return RedirectToAction("PrincipalViewAnnouncements", new { id = principalId });
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n" + ex.Message.ToUpper() + "\n\n");
                return RedirectToAction("Index", "Home");
            }
        }
        
        
        [HttpPost]
        public IActionResult PrincipalUpdateDetails(Principal principal)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _returnDictionary = _principalService.UpdatePrincipalAsync(principal).Result;
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                    return RedirectToAction("PrincipalViewProfile", new { id = principal.Id });
                }
                return View(principal);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n" + ex.Message.ToUpper() + "\n\n");
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
