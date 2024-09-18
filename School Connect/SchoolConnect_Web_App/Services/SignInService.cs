using SchoolConnect_DomainLayer.Models;
using SchoolConnect_Web_App.IServices;
using System.Text;
using System.Text.Json;

namespace SchoolConnect_Web_App.Services
{
    public class SignInService : ISignInService
    {
        private readonly HttpClient _client;
        private const string BasePath = "/api/SignIn/";
        private Dictionary<string, object> _returnDictionary;

        public SignInService(HttpClient client)
        {
            _client = client;
            _returnDictionary = [];
        }

        public Dictionary<string, object> SignInWithEmailAndPassword(LoginModel model)
        {
            try
            {
                StringBuilder buildString = new();
                buildString.Append("http://localhost:5293");
                buildString.Append(BasePath);
                buildString.Append("SignIn");

                var loginmodelJsonString = JsonSerializer.Serialize(model);
                var request = new HttpRequestMessage
                {
                    Content = new StringContent(loginmodelJsonString, Encoding.UTF8, "application/json"),
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(buildString.ToString())
                };

                var response = _client.SendAsync(request).Result;

                if (!response.IsSuccessStatusCode)
                {
                    _returnDictionary = response.Content.ReadFromJsonAsync<Dictionary<string, object>>().Result!;
                    throw new Exception(_returnDictionary["ErrorMessage"] as string ?? "Something went wrong, could not acquire error message from the API.");
                }
                    

                _returnDictionary = response.Content.ReadFromJsonAsync<Dictionary<string, object>>().Result!;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public Dictionary<string, object> SetNewPassword(LoginModel model)
        {
            _returnDictionary = [];
            try
            {
                StringBuilder buildString = new();
                buildString.Append("http://localhost:5293");
                buildString.Append(BasePath);
                buildString.Append("SetNewPassword");

                var jsonString = JsonSerializer.Serialize(model);

                var request = new HttpRequestMessage
                {
                    Content = new StringContent(jsonString, Encoding.UTF8, "application/json"),
                    Method = HttpMethod.Put,
                    RequestUri = new Uri(buildString.ToString())
                };

                var response = _client.SendAsync(request).Result;

                if (!response.IsSuccessStatusCode)
                    throw new Exception(response.Content.ReadAsStringAsync().Result ?? "Something went wrong, could not acquire error content from API");

                _returnDictionary["Success"] = true;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public void SignOut()
        {
            StringBuilder buildString = new();
            buildString.Append(BasePath);
            buildString.Append("SignOut/");

            var response = _client.GetAsync(buildString.ToString()).Result;

            if (!response.IsSuccessStatusCode)
                throw new Exception(response.ReasonPhrase ?? "Operation failed; reason not provided.");
        }
    }
}
