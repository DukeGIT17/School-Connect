using Microsoft.AspNetCore.Mvc;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_Web_App.IServices;

namespace SchoolConnect_Web_App.Controllers
{
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;
        private Dictionary<string, object> _returnDictionary;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
            _returnDictionary = [];
        }

        [HttpPost]
        [Route("/Chat/SendMessage")]
        public IActionResult SendMessage([FromBody] Chat chatMessage)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    chatMessage.TimeSent = DateTime.Now;
                    _returnDictionary = _chatService.SaveMessageAsync(chatMessage).Result;
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                    return Ok(chatMessage);
                }
                return BadRequest(chatMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n" + ex.Message.ToUpper() + "\n\n");
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
