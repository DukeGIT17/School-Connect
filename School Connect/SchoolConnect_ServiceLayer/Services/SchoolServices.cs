using SchoolConnect_DomainLayer.Models;
using SchoolConnect_ServiceLayer.IServices;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace SchoolConnect_ServiceLayer.Services
{
    public class SchoolServices : ISchoolService
    {
        private readonly HttpClient _httpClient;
        private const string SchoolBasePath = "/api/School/";
        private Dictionary<string, object>? _returnDictionary;

        public SchoolServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Dictionary<string, object>> GetSchools()
        {
            _returnDictionary = [];
            try
            {
                StringBuilder buildString = new();
                buildString.Append(SchoolBasePath);
                buildString.Append("Schools/");

                var response = await _httpClient.GetAsync(buildString.ToString());

                if (!response.IsSuccessStatusCode)
                {
                    _returnDictionary.Add("Success", false);
                    _returnDictionary.Add("ErrorMessage", response.ReasonPhrase ?? "Operation failed; reason not provided.");
                    return _returnDictionary;
                }

                var schools = await response.Content.ReadFromJsonAsync<IEnumerable<School>>();
                if (schools == null)
                {
                    _returnDictionary.Add("Success", false);
                    _returnDictionary.Add("ErrorMessage", "Failed to acquire Schools from API.");
                    return _returnDictionary;
                }

                _returnDictionary.Add("Success", true);
                _returnDictionary.Add("Result", schools);
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary.Add("Success", false);
                _returnDictionary.Add("ErrorMessage", ex.Message);
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> RegisterSchool(School school)
        {
            _returnDictionary = [];
            try
            {
                StringBuilder buildString = new();
                buildString.Append("https://localhost:7091");
                buildString.Append(SchoolBasePath);
                buildString.Append("RegisterSchool/");
                var schoolJsonString = JsonSerializer.Serialize(school);

                HttpRequestMessage request = new()
                {
                    Content = new StringContent(schoolJsonString, Encoding.UTF8, "application/json"),
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(buildString.ToString())
                };

                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    _returnDictionary.Add("Success", false);
                    _returnDictionary.Add("ErrorMessage", response.ReasonPhrase ?? "Operation failed; reason not provided.");
                    return _returnDictionary;
                }

                _returnDictionary.Add("Success", true);
                _returnDictionary.Add("Result", response);
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary.Add("Success", false);
                _returnDictionary.Add("ErrorMessage", ex.Message);
                return _returnDictionary;
            }
        }
    }
}
