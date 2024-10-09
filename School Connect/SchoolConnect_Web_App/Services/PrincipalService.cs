using SchoolConnect_DomainLayer.Models;
using SchoolConnect_Web_App.IServices;
using System.Text;
using System.Text.Json;

namespace SchoolConnect_Web_App.Services
{
    public class PrincipalService : IPrincipalService
    {
        private readonly HttpClient _httpClient;
        private const string principalBasePath = "/api/Principal";
        private Dictionary<string, object> _returnDictionary;

        public PrincipalService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _returnDictionary = [];
        }

        public async Task<Dictionary<string, object>> RegisterPrincipalAsync(Principal principal)
        {
            try
            {
                StringBuilder buildString = new();
                buildString.Append("http://localhost:5293");
                buildString.Append(principalBasePath);
                buildString.Append("/Create");

                var request = new HttpRequestMessage
                {
                    Content = new StringContent(JsonSerializer.Serialize(principal), Encoding.UTF8, "application/json"),
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
