using SchoolConnect_DomainLayer.Models;
using SchoolConnect_Web_App.IServices;
using System.Text;
using System.Text.Json;

namespace SchoolConnect_Web_App.Services
{
    public class SchoolService : ISchoolService
    {
        private readonly HttpClient _httpClient;
        private const string SchoolBasePath = "/api/School/";
        private Dictionary<string, object> _returnDictionary;

        public SchoolService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _returnDictionary = [];
        }

        public async Task<Dictionary<string, object>> GetSchools()
        {
            try
            {
                StringBuilder buildString = new();
                buildString.Append("http://localhost:5293");
                buildString.Append(SchoolBasePath);
                buildString.Append("Schools/");

                var response = await _httpClient.GetAsync(buildString.ToString());

                _returnDictionary = SharedClientSideServices.CheckSuccessStatus(response, nameof(GetSchools));
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary.Add("Success", false);
                _returnDictionary.Add("ErrorMessage", ex.Message);
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> RegisterSchoolAsync(School school)
        {
            try
            {
                StringBuilder buildString = new();
                buildString.Append("http://localhost:5293");
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
                

                _returnDictionary = SharedClientSideServices.CheckSuccessStatus(response, "NoNeed");
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetSchoolByAdminAsync(long adminId)
        {
            try
            {
                StringBuilder buildString = new();
                buildString.Append("http://localhost:5293");
                buildString.Append(SchoolBasePath);
                buildString.Append("GetSchoolByAdminId?adminId=");
                buildString.Append(adminId);

                var response = await _httpClient.GetAsync(buildString.ToString());
                return SharedClientSideServices.CheckSuccessStatus(response, nameof(GetSchoolByAdminAsync));
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetSchoolByIdAsync(long schoolId)
        {
            try
            {
                StringBuilder buildString = new();
                buildString.Append("http://localhost:5293");
                buildString.Append(SchoolBasePath);
                buildString.Append("GetSchoolById?schoolId=");
                buildString.Append(schoolId);

                var response = await _httpClient.GetAsync(buildString.ToString());
                return SharedClientSideServices.CheckSuccessStatus(response, nameof(GetSchoolByIdAsync));
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message + "\nInner Exception: " + ex.InnerException;
                return _returnDictionary;
            }
        }
    }
}
