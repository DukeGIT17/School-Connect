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

                    if (!(bool)_resultDictionary["ResetPassword"])
                        return RedirectToAction(nameof(SetNewPassword));

                    return RedirectToAction("SysAdminLandingPage", nameof(SysAdminController));
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
        public IActionResult SetNewPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SetNewPassword(LoginModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(model.NewPassword) || string.IsNullOrWhiteSpace(model.NewPassword))
                    {
                        ModelState.AddModelError(nameof(model.NewPassword), "Please enter a new password");
                        return View(model);
                    }

                    if (string.IsNullOrEmpty(model.ConfirmPassword) || string.IsNullOrWhiteSpace(model.ConfirmPassword))
                    {
                        ModelState.AddModelError(nameof(model.ConfirmPassword), "Please repeat your new password");
                        return View(model);
                    }

                    model.NewPassword = model.NewPassword.Trim();
                    model.ConfirmPassword = model.ConfirmPassword.Trim();
                    model.EmailAddress = model.EmailAddress.Trim();
                    _resultDictionary = _signInService.SetNewPassword(model);

                    if (!(bool)_resultDictionary["Success"]) 
                        throw new Exception(_resultDictionary["ErrorMessage"] as string 
                            ?? "Something went wrong, reason was not provided. Please contact administrators.");

                    return RedirectToAction("SysAdminLandingPage", nameof(SysAdminController));
                }
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }
    }
}
