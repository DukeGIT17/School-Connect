﻿using SchoolConnect_DomainLayer.Models;
using SchoolConnect_Web_App.IServices;
using System.Text;
using System.Text.Json;

namespace SchoolConnect_Web_App.Services
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

        public Dictionary<string, object> GetAdminById(long systemAdminId)
        {
            _returnDictionary = [];
            try
            {
                StringBuilder buildString = new();
                buildString.Append(BasePath);
                buildString.Append("GetSystemAdminById?id=");
                buildString.Append(systemAdminId + "/");

                var response = _client.GetAsync(buildString.ToString()).Result;

                if (!response.IsSuccessStatusCode)
                    throw new Exception(response.ReasonPhrase ?? "Operation failed; reason not provided.");

                var admin = response.Content.ReadFromJsonAsync<SysAdmin>().Result 
                    ?? throw new Exception("Failed to acquire Admin from the API using ID.");

                _returnDictionary["Success"] = true;
                _returnDictionary["Result"] = admin;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }
        
        public Dictionary<string, object> GetAdminByStaffNr(long staffNr)
        {
            _returnDictionary = [];
            try
            {
                StringBuilder buildString = new();
                buildString.Append(BasePath);
                buildString.Append("GetSystemAdminByStaffNr?staffNr=");
                buildString.Append(staffNr + "/");

                var response = _client.GetAsync(buildString.ToString()).Result;

                if (!response.IsSuccessStatusCode)
                    throw new Exception(response.ReasonPhrase ?? "Operation failed; reason not provided.");

                var admin = response.Content.ReadFromJsonAsync<SysAdmin>().Result 
                    ?? throw new Exception("Failed to acquire Admin from the API using staff number.");

                _returnDictionary["Success"] = true;
                _returnDictionary["Result"] = admin;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public Dictionary<string, object> Update(SysAdmin systemAdmin)
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

                var response = _client.SendAsync(request).Result;

                if (!response.IsSuccessStatusCode)
                    throw new Exception(response.ReasonPhrase ?? "Operation failed; reason not provided.");

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
    }
}