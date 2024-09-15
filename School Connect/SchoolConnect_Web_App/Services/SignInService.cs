using SchoolConnect_Web_App.IServices;
using System.Text;

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

        public Dictionary<string, object> SignInWithEmailAndPasswordAsync(string email, string password)
        {
            _returnDictionary = [];
            try
            {
                StringBuilder buildString = new();
                buildString.Append("https://localhost:7091/");
                buildString.Append(BasePath);
                buildString.Append("SignIn?email=");
                buildString.Append(email + "/");

                var request = new HttpRequestMessage
                {
                    Content = new StringContent(password, Encoding.UTF8, "application/json"),
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(buildString.ToString())
                };

                var response = _client.SendAsync(request).Result;

                if (!response.IsSuccessStatusCode)
                    throw new Exception("An error occurred while trying to sign in.");

                _returnDictionary = response.Content.ReadFromJsonAsync<Dictionary<string, object>>().Result
                    ?? throw new Exception("Failed to acquire result dictionary from API.");

                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public void SignOutAsync()
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
