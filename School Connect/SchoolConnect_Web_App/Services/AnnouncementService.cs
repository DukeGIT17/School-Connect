using SchoolConnect_DomainLayer.Models;
using SchoolConnect_RepositoryLayer.Interfaces;
using SchoolConnect_Web_App.IServices;
using System.Text;
using System.Text.Json;

namespace SchoolConnect_Web_App.Services
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly HttpClient _httpClient;
        private const string announcementBasePath = "/api/Announcement";
        private Dictionary<string, object> _returnDictionary;

        public AnnouncementService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _returnDictionary = [];
        }

        public async Task<Dictionary<string, object>> CreateAnnouncementAsync(Announcement announcement)
        {
            try
            {
                StringBuilder buildString = new();
                buildString.Append("http://localhost:5293");
                buildString.Append(announcementBasePath);
                buildString.Append("/Create");

                var request = new HttpRequestMessage
                {
                    Content = new StringContent(JsonSerializer.Serialize(announcement), Encoding.UTF8, "application/json"),
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(buildString.ToString())
                };

                var response = await _httpClient.SendAsync(request);
                return SharedClientSideServices.CheckSuccessStatus(response, "NoNeed");
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetAnnouncementByPrincipalIdAsync(long principalId)
        {
            try
            {
                StringBuilder buildString = new();
                buildString.Append("http://localhost:5293");
                buildString.Append(announcementBasePath);
                buildString.Append("/GetAnnouncementByPrincipalId?principalId=");
                buildString.Append(principalId);

                var response = await _httpClient.GetAsync(buildString.ToString());
                return SharedClientSideServices.CheckSuccessStatus(response, nameof(GetAnnouncementByPrincipalIdAsync));
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
