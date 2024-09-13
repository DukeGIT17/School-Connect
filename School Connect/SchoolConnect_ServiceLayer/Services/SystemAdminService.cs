using SchoolConnect_DomainLayer.Models;
using SchoolConnect_ServiceLayer.IServices;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace SchoolConnect_ServiceLayer.Services
{
    public class SystemAdminService : ISystemAdminService
    {
        public readonly HttpClient _client;
        public const string BasePath = "/api/SystemAdmin/";
        public Dictionary<string, object> _returnDictionary;

        public SystemAdminService(HttpClient client)
        {
            _client = client;
            _returnDictionary = [];
        }

        public async Task<Dictionary<string, object>> CreateAdmin(SysAdmin systemAdmin)
        {
            _returnDictionary = [];
            try
            {
                StringBuilder buildString = new();
                buildString.Append("https://localhost:7091");
                buildString.Append(BasePath);
                buildString.Append("CreateAdmin/");

                var adminJsonString = JsonSerializer.Serialize(systemAdmin);

                var request = new HttpRequestMessage
                {
                    Content = new StringContent(adminJsonString, Encoding.UTF8, "application/json"),
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(buildString.ToString())
                };

                var response = await _client.SendAsync(request);
                
                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = response.ReasonPhrase ?? "Operation failed; reason not provided.";
                    errorMessage += " {" + response.StatusCode + "}";

                    _returnDictionary.Add("Success", false);
                    _returnDictionary.Add("ErrorMessage", errorMessage);
                    return _returnDictionary;
                }

                _returnDictionary.Add("Success", true);
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary.Add("Success", false);
                _returnDictionary.Add("ErrorMessage", ex.Message);
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetAdminById(long systemAdminId)
        {
            _returnDictionary = [];
            try
            {
                StringBuilder buildString = new();
                buildString.Append(BasePath);
                buildString.Append("GetSystemAdminById?id=");
                buildString.Append(systemAdminId + "/");

                var response = await _client.GetAsync(buildString.ToString());

                if (!response.IsSuccessStatusCode)
                {
                    _returnDictionary.Add("Success", false);
                    _returnDictionary.Add("ErrorMessage", response.ReasonPhrase ?? "Operation failed; reason not provided.");
                    return _returnDictionary;
                }

                var admin = await response.Content.ReadFromJsonAsync<SysAdmin>();

                if (admin == null)
                {
                    _returnDictionary.Add("Success", false);
                    _returnDictionary.Add("ErrorMessage", "Failed to acquire Admin from the API using ID.");
                    return _returnDictionary;
                }

                _returnDictionary.Add("Success", true);
                _returnDictionary.Add("Result", admin);
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary.Add("Success", false);
                _returnDictionary.Add("ErrorMessage", ex.Message);
                return _returnDictionary;
            }
        }
        
        public async Task<Dictionary<string, object>> GetAdminByStaffNr(long staffNr)
        {
            _returnDictionary = [];
            try
            {
                StringBuilder buildString = new();
                buildString.Append(BasePath);
                buildString.Append("GetSystemAdminByStaffNr?staffNr=");
                buildString.Append(staffNr + "/");

                var response = await _client.GetAsync(buildString.ToString());

                if (!response.IsSuccessStatusCode)
                {
                    _returnDictionary.Add("Success", false);
                    _returnDictionary.Add("ErrorMessage", response.ReasonPhrase ?? "Operation failed; reason not provided.");
                    return _returnDictionary;
                }

                var admin = await response.Content.ReadFromJsonAsync<SysAdmin>();

                if (admin == null)
                {
                    _returnDictionary.Add("Success", false);
                    _returnDictionary.Add("ErrorMessage", "Failed to acquire Admin from the API using staff number.");
                    return _returnDictionary;
                }

                _returnDictionary.Add("Success", true);
                _returnDictionary.Add("Result", admin);
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary.Add("Success", false);
                _returnDictionary.Add("ErrorMessage", ex.Message);
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> Update(SysAdmin systemAdmin)
        {
            _returnDictionary = [];
            try
            {
                StringBuilder buildString = new();
                buildString.Append("https://localhost:7091");
                buildString.Append(BasePath);
                buildString.Append("UpdateSystemAdmin/");

                var schoolJsonString = JsonSerializer.Serialize(systemAdmin);

                HttpRequestMessage request = new()
                {
                    Content = new StringContent(schoolJsonString, Encoding.UTF8, "application/json"),
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(buildString.ToString())
                };

                var response = await _client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    _returnDictionary.Add("Success", false);
                    _returnDictionary.Add("ErrorMessage", response.ReasonPhrase ?? "Operation failed; reason not provided.");
                    return _returnDictionary;
                }

                _returnDictionary.Add("Success", true);
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
