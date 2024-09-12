using SchoolConnect_DomainLayer.Models;
using SchoolConnect_ServiceLayer.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SchoolConnect_ServiceLayer.Services
{
    internal class SystemAdminService : ISystemAdminService
    {
        public readonly HttpClient _client;
        public const string BasePath = "/api/SystemAdmin/";
        public Dictionary<string, object> _returnDictionary;

        public SystemAdminService(HttpClient client)
        {
            _client = client;
            _returnDictionary = [];
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
                response.EnsureSuccessStatusCode();
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
                response.EnsureSuccessStatusCode();
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
                response.EnsureSuccessStatusCode();

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
