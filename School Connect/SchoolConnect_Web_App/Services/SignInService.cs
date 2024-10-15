using Microsoft.AspNetCore.Identity;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_Web_App.IServices;
using System.Text;
using System.Text.Json;

namespace SchoolConnect_Web_App.Services
{
    public class SignInService : ISignInService
    {
        private readonly HttpClient _client;
        private readonly SignInManager<CustomIdentityUser> _signInManager;
        private const string BasePath = "/api/SignIn/";
        private Dictionary<string, object> _returnDictionary;

        public SignInService(HttpClient client, SignInManager<CustomIdentityUser> signInManager)
        {
            _client = client;
            _signInManager = signInManager;
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
                    _ = string.IsNullOrEmpty(response.Content.ReadAsStringAsync().Result) ? throw new Exception("Response from the API was null or empty.") : "";
                    throw new Exception(response.Content.ReadAsStringAsync().Result);
                }

                _returnDictionary = SharedClientSideServices.ConvertJsonElements(response.Content.ReadFromJsonAsync<Dictionary<string, object>>().Result!);
                if ((bool)_returnDictionary["Success"])
                    _signInManager.SignInAsync(_signInManager.UserManager.FindByEmailAsync(model.EmailAddress).Result, false);
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
                    throw new Exception(response.Content.ReadAsStringAsync().Result 
                        ?? "Something went wrong, could not acquire error content from API");


                _returnDictionary = response.Content.ReadFromJsonAsync<Dictionary<string, object>>().Result!;
                _returnDictionary = SharedClientSideServices.ConvertJsonElements(_returnDictionary);
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
