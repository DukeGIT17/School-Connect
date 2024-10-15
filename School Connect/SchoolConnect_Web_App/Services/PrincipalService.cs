using SchoolConnect_DomainLayer.Models;
using SchoolConnect_Web_App.IServices;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SchoolConnect_Web_App.Services
{
    public class PrincipalService : IPrincipalService
    {
        private readonly HttpClient _httpClient;
        private const string principalBasePath = "/api/Principal";
        private Dictionary<string, object> _returnDictionary;

        public PrincipalService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _returnDictionary = [];
        }

        public async Task<Dictionary<string, object>> RegisterPrincipalAsync(Principal principal)
        {
            try
            {
                StringBuilder buildString = new();
                buildString.Append("http://localhost:5293");
                buildString.Append(principalBasePath);
                buildString.Append("/Create");

                var formData = new MultipartFormDataContent();
                
                foreach (var property in typeof(Principal).GetProperties())
                {
                    var value = property.GetValue(principal);
                    if (value is IFormFile)
                        continue;
                    if (value is not null)
                        formData.Add(new StringContent(value.ToString()), property.Name);
                }

                if (principal.ProfileImageFile is not null)
                {
                    var fileStreamContent = new StreamContent(principal.ProfileImageFile.OpenReadStream());
                    fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(principal.ProfileImageFile.ContentType);
                    formData.Add(fileStreamContent, "ProfileImageFile", principal.ProfileImageFile.FileName);
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

        public async Task<Dictionary<string, object>> GetPrincipalByIdAsync(long principalId)
        {
            try
            {
                StringBuilder buildString = new();
                buildString.Append("http://localhost:5293");
                buildString.Append(principalBasePath);
                buildString.Append("/GetPrincipalById?id=");
                buildString.Append(principalId);

                var response = await _httpClient.GetAsync(buildString.ToString());
                return SharedClientSideServices.CheckSuccessStatus(response, nameof(GetPrincipalByIdAsync));
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
