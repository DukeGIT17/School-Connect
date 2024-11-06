using SchoolConnect_DomainLayer.Models;
using SchoolConnect_RepositoryLayer.Interfaces;
using SchoolConnect_ServiceLayer.IServerSideServices;

namespace SchoolConnect_ServiceLayer.ServerSideServices
{
    public class ChatService : IChatService
    {
        private readonly IChats _chatRepo;
        private Dictionary<string, object> _returnDictionary;

        public ChatService(IChats chatRepo)
        {
            _chatRepo = chatRepo;
            _returnDictionary = [];
        }

        public async Task<Dictionary<string, object>> SaveMessageAsync(Chat chatMessage)
        {
            try
            {
                return await _chatRepo.SaveMessageAsync(chatMessage);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }
    }
}
