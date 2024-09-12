using Microsoft.AspNetCore.Mvc;

namespace schoolconnect.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Chat()
        {
            return View();
        }
    }
}
