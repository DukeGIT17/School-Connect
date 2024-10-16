using Microsoft.IdentityModel.Tokens;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_Web_App.IServices;
using System.Text;
using System.Net.Http.Headers;
using static SchoolConnect_Web_App.Services.SharedClientSideServices;

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

        public async Task<Dictionary<string, object>> GetAdminByIdAsync(long systemAdminId)
        {
            _returnDictionary = [];
            try
            {
                StringBuilder buildString = new();
                buildString.Append("http://localhost:5293");
                buildString.Append(BasePath);
                buildString.Append("GetSystemAdminById?id=");
                buildString.Append(systemAdminId);

                var response = await _client.GetAsync(buildString.ToString());
				_returnDictionary = CheckSuccessStatus(response, nameof(GetAdminByIdAsync));
				return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }
        
        public async Task<Dictionary<string, object>> GetAdminByStaffNr(string staffNr)
        {
            _returnDictionary = [];
            try
            {
                StringBuilder buildString = new();
                buildString.Append(BasePath);
                buildString.Append("GetSystemAdminByStaffNr?staffNr=");
                buildString.Append(staffNr);

                var response = await _client.GetAsync(buildString.ToString());
                return CheckSuccessStatus(response, nameof(GetAdminByStaffNr));
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> UpdateAsync(SysAdmin systemAdmin)
        {
            _returnDictionary = [];
            try
            {
                StringBuilder buildString = new();
                buildString.Append("http://localhost:5293");
                buildString.Append(BasePath);
                buildString.Append("UpdateSystemAdmin/");

                var formData = new MultipartFormDataContent();

                foreach (var property in typeof(SysAdmin).GetProperties())
                {
                    var value = property.GetValue(systemAdmin);
                    if (value is IFormFile)
                        continue;

                    if (value is not null)
                        formData.Add(new StringContent(value.ToString()), property.Name);
                }

                if (systemAdmin.ProfileImageFile is not null)
                {
                    var fileStreamContent = new StreamContent(systemAdmin.ProfileImageFile.OpenReadStream());
                    fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(systemAdmin.ProfileImageFile.ContentType);
                    formData.Add(fileStreamContent, "ProfileImageFile", systemAdmin.ProfileImageFile.FileName);
                }

                HttpRequestMessage request = new()
                {
                    Content = formData,
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(buildString.ToString())
                };

                var response = await _client.SendAsync(request);
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
