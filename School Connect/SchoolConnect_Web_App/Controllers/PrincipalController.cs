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
        private Dictionary<string, object> _returnDictionary;

        public PrincipalController(IPrincipalService principalService, IAnnouncementService announcementService)
        {
            _principalService = principalService;
            _announcementService = announcementService;
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

                var principal = _returnDictionary["Result"] as Principal;
                ActorAnnouncementViewModel<Principal> model = new()
                {
                    Actor = principal!,
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
                    _returnDictionary = _announcementService.CreateAnnouncementAsync(model.Announcement).Result;
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                    return RedirectToAction(nameof(PrincipalLandingPage), new { id = model.Announcement.PrincipalID });
                }

                Principal principal = new();
                principal.Id = (long)model.Announcement.PrincipalID!;
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
                return View(_returnDictionary["Result"]);

            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n" + ex.Message.ToUpper() + "\n\n");
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult PrincipalViewGrades()
		{
			return View();
		}

        public IActionResult PrincipalGradeLandingPage()
        {
            return View();
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
        public IActionResult PrincipalDetailedAnnouncement(int id)
        {
            try
            {
                _returnDictionary = _announcementService.GetAnnouncementById(id).Result;
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
