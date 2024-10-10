using SchoolConnect_DomainLayer.Models;
using SchoolConnect_Web_App.IServices;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Text.Json;

namespace SchoolConnect_Web_App.Services
{
    public class LearnerService : ILearnerService
    {
        private readonly HttpClient _httpClient;
        private const string learnerBasePath = "/api/Learner";
        private Dictionary<string, object> _returnDictionary;

        public LearnerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _returnDictionary = [];
        }

        public async Task<Dictionary<string, object>> RegisterLearnerAsync(Learner learner)
        {
            try
            {
                StringBuilder buildString = new();
                buildString.Append("http://localhost:5293");
                buildString.Append(learnerBasePath);
                buildString.Append("/Create");

                var request = new HttpRequestMessage
                {
                    Content = new StringContent(JsonSerializer.Serialize(learner), Encoding.UTF8, "application/json"),
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
    }
}
