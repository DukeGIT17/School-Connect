using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_ServiceLayer.IServices;

namespace schoolconnect.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ISchoolService _schoolService;

        public LoginController(SignInManager<IdentityUser> signInManager, ISchoolService schoolService)
        {
            _signInManager = signInManager;
            _schoolService = schoolService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _signInManager.UserManager.FindByEmailAsync(model.EmailAddress) ?? throw new Exception("Invalid Credentials.");
                    var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

                    if (!signInResult.Succeeded)
                        throw new Exception("Invalid Credentials.");

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
