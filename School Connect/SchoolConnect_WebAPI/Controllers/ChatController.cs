using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_ServiceLayer.IServerSideServices;

namespace SchoolConnect_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private Dictionary<string, object> _returnDictionary;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
            _returnDictionary = [];
        }

        [HttpPost(nameof(SaveChatMessage))]
        public IActionResult SaveChatMessage(Chat chatMessage)
        {
            try
            {
                _returnDictionary = _chatService.SaveMessageAsync(chatMessage).Result;
                if (!(bool)_returnDictionary["Success"]) return BadRequest(_returnDictionary["ErrorMessage"]);
                return Ok(_returnDictionary);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
