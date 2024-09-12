using Microsoft.AspNetCore.Mvc;
using SchoolConnect_DomainLayer.Models;

namespace schoolconnect.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            return View(model);
        }

        [HttpGet]
        public IActionResult SecondLogin()
        {
            return View();
        }
    }
}
