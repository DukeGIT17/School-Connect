using SchoolConnect_DomainLayer.Models;
using SchoolConnect_Web_App.IServices;
using System.Text;
using System.Net.Http.Headers;
using static SchoolConnect_Web_App.Services.SharedClientSideServices;

namespace SchoolConnect_Web_App.Services
{
    public class LearnerService : ILearnerService
    {
        private readonly HttpClient _httpClient;
        private const string learnerBasePath = "/api/Learner";
        private Dictionary<string, object> _returnDictionary;

        public LearnerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _returnDictionary = [];
        }

        public async Task<Dictionary<string, object>> RegisterLearnerAsync(Learner learner)
        {
            try
            {
                StringBuilder buildString = new();
                buildString.Append("http://localhost:5293");
                buildString.Append(learnerBasePath);
                buildString.Append("/Create");

                var formData = new MultipartFormDataContent();

                foreach (var property in typeof(Learner).GetProperties())
                {
                    var value = property.GetValue(learner);
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

                    if (value is IEnumerable<LearnerParent>)
                    {
                        foreach (var lp in typeof(LearnerParent).GetProperties())
                        {
                            var lpValue = lp.GetValue(learner.Parents.First());
                            if (lpValue is not null && lpValue is not Parent && lpValue is not Learner)
                                formData.Add(new StringContent(lpValue.ToString()), "Parents[0]." + lp.Name);

                            if (lpValue is Parent)
                            {
                                foreach (var parent in typeof(Parent).GetProperties())
                                {
                                    var parentValue = parent.GetValue(learner.Parents.First().Parent);
                                    if (parentValue is IFormFile)
                                        continue;

                                    if (parentValue is not null)
                                        formData.Add(new StringContent(parentValue.ToString()), "Parents[0].Parent." + parent.Name);
                                }
                            }
                            
                        }
                        continue;
                    }

                    if (value is not null)
                        formData.Add(new StringContent(value.ToString()), property.Name);
                }

                if (learner.ProfileImageFile is not null)
                {
                    var fileStreamContent = new StreamContent(learner.ProfileImageFile.OpenReadStream());
                    fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(learner.ProfileImageFile.ContentType);
                    formData.Add(fileStreamContent, "ProfileImageFile", learner.ProfileImageFile.FileName);
                }

                var parentImage = learner.Parents.FirstOrDefault()?.Parent?.ProfileImageFile;
                if (parentImage is not null)
                {
                    var fileStreamContent = new StreamContent(parentImage.OpenReadStream());
                    fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(parentImage.ContentType);
                    formData.Add(fileStreamContent, "Parents[0].Parent.ProfileImageFile", parentImage.FileName);
                }

                var request = new HttpRequestMessage
                {
                    Content = formData,
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(buildString.ToString())
                };

                var response = await _httpClient.SendAsync(request);
                return SharedClientSideServices.CheckSuccessStatus(response, "NoNeed");
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetLearnerByIdNo(string idNo)
        {
            try
            {
                StringBuilder builString = new();
                builString.Append("http://localhost:5293");
                builString.Append(learnerBasePath);
                builString.Append("/GetLearnerByIdNo?=");
                builString.Append(idNo);

                var response = await _httpClient.GetAsync(builString.ToString());
                return CheckSuccessStatus(response, nameof(GetLearnerByIdNo));
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> BulkLoadLearnersAsync(IFormFile file, long schoolId)
        {
            try
            {
                StringBuilder builString = new();
                builString.Append("http://localhost:5293");
                builString.Append(learnerBasePath);
                builString.Append("/LoadLearnersFromExcel?schoolId=");
                builString.Append(schoolId);

                var formData = new MultipartFormDataContent();

                var fileStreamContent = new StreamContent(file.OpenReadStream());
                fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                formData.Add(fileStreamContent, "file", file.FileName);

                var request = new HttpRequestMessage
                {
                    Content = formData,
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(builString.ToString())
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
