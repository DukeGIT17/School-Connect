using Microsoft.AspNetCore.Mvc;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_Web_App.IServices;

namespace schoolconnect.Controllers
{
    public class LoginController : Controller
    {
        private readonly ISchoolService _schoolService;
        private readonly ISignInService _signInService;
        private Dictionary<string, object> _resultDictionary;

        public LoginController(ISchoolService schoolService, ISignInService signInService)
        {
            _schoolService = schoolService;
            _signInService = signInService;
            _resultDictionary = [];
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _resultDictionary = _signInService.SignInWithEmailAndPassword(model);

                    if (!(bool)_resultDictionary["Success"])
                        throw new Exception($"Invalid Credentials. Issue: {_resultDictionary["ErrorMessage"]}");

                    return RedirectToAction(nameof(SecondLogin));
                }                
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult SecondLogin()
        {
            return View();
        }
    }
}
