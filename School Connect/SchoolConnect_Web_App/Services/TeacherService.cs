using SchoolConnect_DomainLayer.Models;
using SchoolConnect_RepositoryLayer.Interfaces;
using SchoolConnect_Web_App.IServices;
using System.Net.Http.Headers;
using System.Text;
using static SchoolConnect_Web_App.Services.SharedClientSideServices;

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

                var formData = new MultipartFormDataContent();
                foreach (var property in typeof(Teacher).GetProperties())
                {
                    var value = property.GetValue(teacher);
                    if (value is IFormFile)
                        continue;

                    if (value is IEnumerable<string> enumerableValue && value is not string)
                    {
                        int index = 0;
                        foreach (var item in enumerableValue)
                        {
                            if (item is not null)
                            {
                                formData.Add(new StringContent(item.ToString()), $"{property.Name}[{index}]");
                                index++;
                            }
                        }
                        continue;
                    }

                    if (value is not null)
                        formData.Add(new StringContent(value.ToString()), property.Name);
                }

                if (teacher.ProfileImageFile is not null)
                {
                    var fileStreamContent = new StreamContent(teacher.ProfileImageFile.OpenReadStream());
                    fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(teacher.ProfileImageFile.ContentType);
                    formData.Add(fileStreamContent, "ProfileImageFile", teacher.ProfileImageFile.FileName);
                }

                var request = new HttpRequestMessage()
                {
                    Content = formData,
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
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMEssage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> BulkLoadTeachersAsync(IFormFile teachersFile, long schoolId)
        {
            try
            {
                StringBuilder buildString = new();
                buildString.Append("http://localhost:5293");
                buildString.Append(teacherBasePath);
                buildString.Append("/BulkLoadTeachersFromExcel?schoolId=");
                buildString.Append(schoolId);

                var formData = new MultipartFormDataContent();

                var fileStreamContent = new StreamContent(teachersFile.OpenReadStream());
                fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(teachersFile.ContentType);
                formData.Add(fileStreamContent, "file", teachersFile.FileName);

                var request = new HttpRequestMessage
                {
                    Content = formData,
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(buildString.ToString())
                };

                var response = await _httpClient.SendAsync(request);
                return CheckSuccessStatus(response, "NoNeed");
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetTeacherByIdAsync(long teacherId)
        {
            try 
            {
                StringBuilder buildString = new();
                buildString.Append("http://localhost:5293");
                buildString.Append(teacherBasePath);
                buildString.Append("/GetTeacherById?id=");
                buildString.Append(teacherId);

                var response = await _httpClient.GetAsync(buildString.ToString());
                return CheckSuccessStatus(response, "Teacher");
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> UpdateTeacherAsync(Teacher teacher)
        {
            try
            {
                StringBuilder buildString = new();
                buildString.Append("http://localhost:5293");
                buildString.Append(teacherBasePath);
                buildString.Append("/UpdateTeacher");

                var formData = new MultipartFormDataContent();

                foreach (var property in typeof(Teacher).GetProperties())
                {
                    var value = property.GetValue(teacher);
                    if (value is IFormFile)
                        continue;

                    if (value is IEnumerable<string> enumerableValue && value is not string)
                    {
                        int index = 0;
                        foreach (var item in enumerableValue)
                        {
                            if (item is not null)
                            {
                                formData.Add(new StringContent(item.ToString()), $"{property.Name}[{index}]");
                                index++;
                            }
                        }
                        continue;
                    }

                    if (value is not null)
                        formData.Add(new StringContent(value.ToString()), property.Name);
                }

                if (teacher.ProfileImageFile is not null)
                {
                    var fileStreamContent = new StreamContent(teacher.ProfileImageFile.OpenReadStream());
                    fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(teacher.ProfileImageFile.ContentType);
                    formData.Add(fileStreamContent, "ProfileImageFile", teacher.ProfileImageFile.FileName);
                }

                var request = new HttpRequestMessage
                {
                    Content = formData,
                    Method = HttpMethod.Put,
                    RequestUri = new Uri(buildString.ToString())
                };

                var response = await _httpClient.SendAsync(request);
                return CheckSuccessStatus(response, "NoNeed");
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
