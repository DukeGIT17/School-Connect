using SchoolConnect_DomainLayer.Models;
using SchoolConnect_Web_App.IServices;
using System.Text;
using System.Net.Http.Headers;
using static SchoolConnect_Web_App.Services.SharedClientSideServices;

namespace SchoolConnect_Web_App.Services
{
    public class ParentService(HttpClient httpClient) : IParentService
    {
        private Dictionary<string, object> _returnDictionary = [];
        private const string ParentBasePath = "/api/Parent";
        public async Task<Dictionary<string, object>> RegisterParentAsync(Parent parent)
        {
            try
            {
                StringBuilder buildString = new();
                buildString.Append("http://localhost:5293");
                buildString.Append(ParentBasePath);
                buildString.Append("/Create");

                var formData = new MultipartFormDataContent();

                foreach (var property in typeof(Parent).GetProperties())
                {
                    var value = property.GetValue(parent);
                    if (value is IFormFile)
                        continue;

                    if (value is IEnumerable<LearnerParent>)
                    {
                        foreach (var learnerParentProperty in typeof(LearnerParent).GetProperties())
                        {
                            var lpValue = learnerParentProperty.GetValue(parent.Children!.First());
                            if (lpValue is IFormFile || lpValue is Learner || lpValue is Parent)
                                continue;

                            if (lpValue is not null)
                                formData.Add(new StringContent(lpValue.ToString()), "Children[0]." + learnerParentProperty.Name);
                        }
                        continue;
                    }

                    if (value is not null)
                        formData.Add(new StringContent(value.ToString()), property.Name);
                }

                if (parent.ProfileImageFile is not null)
                {
                    var fileStreamContent = new StreamContent(parent.ProfileImageFile.OpenReadStream());
                    fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(parent.ProfileImageFile.ContentType);
                    formData.Add(fileStreamContent, "ProfileImageFile", parent.ProfileImageFile.FileName);
                }

                var request = new HttpRequestMessage
                {
                    Content = formData,
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(buildString.ToString())
                };

                var response = await httpClient.SendAsync(request);
                return CheckSuccessStatus(response, "NoNeed");
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> BulkLoadParentsAsync(IFormFile file)
        {
            try
            {
                StringBuilder buildString = new();
                buildString.Append("http://localhost:5293");
                buildString.Append(ParentBasePath);
                buildString.Append("/BatchLoadParentsFromExcel");

                var formData = new MultipartFormDataContent();

                var fileStreamContent = new StreamContent(file.OpenReadStream());
                fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                formData.Add(fileStreamContent, "file", file.FileName);

                var request = new HttpRequestMessage
                {
                    Content = formData,
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(buildString.ToString())
                };

                var response = await httpClient.SendAsync(request);
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
