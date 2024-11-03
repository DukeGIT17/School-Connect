using SchoolConnect_DomainLayer.Models;
using SchoolConnect_Web_App.IServices;
using System.Text;
using static SchoolConnect_Web_App.Services.SharedClientSideServices;
using System.Net.Http.Headers;
using System.Text.Json;

namespace SchoolConnect_Web_App.Services
{
    public class SchoolService : ISchoolService
    {
        private readonly HttpClient _httpClient;
        private const string SchoolBasePath = "/api/School";
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
                buildString.Append("/Schools/");

                var response = await _httpClient.GetAsync(buildString.ToString());

                _returnDictionary = CheckSuccessStatus(response, nameof(GetSchools));
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
                buildString.Append("/RegisterSchool/");

                var formData = new MultipartFormDataContent();

                foreach (var property in typeof(School).GetProperties())
                {
                    var value = property.GetValue(school);
                    if (value is IFormFile)
                        continue;

                    if (value is Address)
                    {
                        foreach (var addressProperty in typeof(Address).GetProperties())
                        {
                            var address = addressProperty.GetValue(school.SchoolAddress);
                            formData.Add(new StringContent(address is not null ? address.ToString()! : string.Empty), $"SchoolAddress.{addressProperty.Name}");
                        }
                        continue;
                    }

                    if (value is not null)
                        formData.Add(new StringContent(value.ToString()), property.Name);
                }

                if (school.SchoolLogoFile is not null)
                {
                    var fileStreamContent = new StreamContent(school.SchoolLogoFile.OpenReadStream());
                    fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(school.SchoolLogoFile.ContentType);
                    formData.Add(fileStreamContent, "SchoolLogoFile", school.SchoolLogoFile.FileName);
                }

                HttpRequestMessage request = new()
                {
                    Content = formData,
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(buildString.ToString())
                };

                var response = await _httpClient.SendAsync(request);
                

                _returnDictionary = CheckSuccessStatus(response, "NoNeed");
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
                buildString.Append("/GetSchoolByAdminId?adminId=");
                buildString.Append(adminId);

                var response = await _httpClient.GetAsync(buildString.ToString());
                return CheckSuccessStatus(response, "School");
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
                buildString.Append("/GetSchoolById?schoolId=");
                buildString.Append(schoolId);

                var response = await _httpClient.GetAsync(buildString.ToString());
                return CheckSuccessStatus(response, "School");
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message + "\nInner Exception: " + ex.InnerException;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetSchoolByLearnerIdNoAsync(string learnerIdNo)
        {
            try
            {
                StringBuilder buildString = new();
                buildString.Append("http://localhost:5293");
                buildString.Append(SchoolBasePath);
                buildString.Append("/GetSchoolByLearnerIdNo?=");
                buildString.Append(learnerIdNo);

                var response = await _httpClient.GetAsync(buildString.ToString());
                return CheckSuccessStatus(response, "School");
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message + "\nInner Exception: " + ex.InnerException;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> UpdateSchoolAsync(School school)
        {
            try
            {
                StringBuilder buildString = new();
                buildString.Append("http://localhost:5293");
                buildString.Append(SchoolBasePath);
                buildString.Append("/UpdateSchool");

                var formData = new MultipartFormDataContent();

                foreach (var property in typeof(School).GetProperties())
                {
                    var value = property.GetValue(school);
                    if (value is IFormFile)
                        continue;

                    if (value is Address address)
                    {
                        foreach (var addressProperty in typeof(Address).GetProperties())
                        {
                            var addressValue = addressProperty.GetValue(address);
                            if (addressValue is not null)
                                formData.Add(new StringContent(addressValue.ToString()), $"{property.Name}.{addressProperty.Name}");
                        }
                    }

                    if (value is not null)
                        formData.Add(new StringContent(value.ToString()), property.Name);
                }

                if (school.SchoolLogoFile is not null)
                {
                    var fileStreamContent = new StreamContent(school.SchoolLogoFile.OpenReadStream());
                    fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(school.SchoolLogoFile.ContentType);
                    formData.Add(fileStreamContent, "SchoolLogoFile", school.SchoolLogoFile.FileName);
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

        public async Task<Dictionary<string, object>> GetAllClassesBySchoolAsync(long schoolId)
        {
            try
            {
                StringBuilder buildString = new();
                buildString.Append("http://localhost:5293");
                buildString.Append(SchoolBasePath);
                buildString.Append("/GetAllClassesBySchool?schoolId=");
                buildString.Append(schoolId);

                var response = await _httpClient.GetAsync(buildString.ToString());
                return CheckSuccessStatus(response, "SubGrade");
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetClassBySchoolAsync(string classDesignate, long schoolId)
        {
            try
            {
                StringBuilder buildString = new();
                buildString.Append("http://localhost:5293");
                buildString.Append(SchoolBasePath);
                buildString.Append("/GetClassBySchool?classDesignate=");
                buildString.Append(classDesignate);
                buildString.Append("&schoolId=");
                buildString.Append(schoolId);

                var response = await _httpClient.GetAsync(buildString.ToString());
                return CheckSuccessStatus(response, "SubGrade");
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> AddClassesToSchool(List<string> classDesignates, long schoolId)
        {
            try
            {
                StringBuilder buildString = new();
                buildString.Append("http://localhost:5293");
                buildString.Append(SchoolBasePath);
                buildString.Append("/AddClassesToSchool?schoolId=");
                buildString.Append(schoolId);

                var request = new HttpRequestMessage
                {
                    Content = new StringContent(JsonSerializer.Serialize(classDesignates), Encoding.UTF8, "application/json"),
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

        public async Task<Dictionary<string, object>> AddSubjectsAsync(List<string> subjects, long schoolId, string? newClasses = null)
        {
            try
            {
                StringBuilder buildString = new();
                buildString.Append("http://localhost:5293");
                buildString.Append(SchoolBasePath);
                buildString.Append("/AddSubjectsToSchool?schoolId=");
                buildString.Append(schoolId);
                buildString.Append("&newClasses=");
                buildString.Append(newClasses);

                var request = new HttpRequestMessage
                {
                    Content = new StringContent(JsonSerializer.Serialize(subjects), Encoding.UTF8, "application/json"),
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

        public async Task<Dictionary<string, object>> GetGradesBySchoolAsync(long schoolId)
        {
            try
            {
                StringBuilder buildString = new();
                buildString.Append("http://localhost:5293");
                buildString.Append(SchoolBasePath);
                buildString.Append("/GetGradesBySchool?schoolId=");
                buildString.Append(schoolId);

                var response = await _httpClient.GetAsync(buildString.ToString());
                return CheckSuccessStatus(response, "Grade");
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetClassByMainTeacherAsync(long teacherId)
        {
            try
            {
                StringBuilder buildString = new();
                buildString.Append("http://localhost:5293");
                buildString.Append(SchoolBasePath);
                buildString.Append("/GetClassByMainTeacher?teacherId=");
                buildString.Append(teacherId);

                var response = await _httpClient.GetAsync(buildString.ToString());
                return CheckSuccessStatus(response, "SubGrade");
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
