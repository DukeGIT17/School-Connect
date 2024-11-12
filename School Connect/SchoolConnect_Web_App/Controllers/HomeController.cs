using Microsoft.AspNetCore.Mvc;
using SchoolConnect_Web_App.Models;
using System.Diagnostics;

namespace SchoolConnect_Web_App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ErrorPage(string errorMessage)
        {
            if (errorMessage.Contains("Unique Constraint Failed", StringComparison.OrdinalIgnoreCase))
                errorMessage = "A value that is supposed to be unique has been entered. Please ensure unique values like identity numbers, staff numbers, email addresses, and phone numbers are correct.";

            ViewBag.ErrorMessage = errorMessage.Replace("Inner Exception:", "");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
