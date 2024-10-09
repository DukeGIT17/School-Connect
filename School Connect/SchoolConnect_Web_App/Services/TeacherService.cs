using SchoolConnect_DomainLayer.Models;
using SchoolConnect_Web_App.IServices;
using System.Text;
using System.Text.Json;

namespace SchoolConnect_Web_App.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly HttpClient _httpClient;
        private const string teacherBasePath = "/api/Teacher";
        private Dictionary<string, object> _returnDictionary;

        public TeacherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _returnDictionary = [];
        }

        public async Task<Dictionary<string, object>> RegisterTeacherAsync(Teacher teacher)
        {
            try
            {
                StringBuilder buildString = new();
                buildString.Append("http://localhost:5293");
                buildString.Append(teacherBasePath);
                buildString.Append("/Create");

                var request = new HttpRequestMessage()
                {
                    Content = new StringContent(JsonSerializer.Serialize(teacher), Encoding.UTF8, "application/json"),
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(buildString.ToString())
                };

                var response = await _httpClient.SendAsync(request);
                _returnDictionary= SharedClientSideServices.CheckSuccessStatus(response, "NoNeed");
                if (!(bool)_returnDictionary["Success"]) 
                    throw new(_returnDictionary["ErrorMessage"] as string);

                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = true;
                _returnDictionary["ErrorMEssage"] = ex.Message;
                return _returnDictionary;
            }
        }
    }
}
