using Microsoft.AspNetCore.Mvc;

namespace SchoolConnect_Web_App.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Chat()
        {
            return View();
        }
    }
}
