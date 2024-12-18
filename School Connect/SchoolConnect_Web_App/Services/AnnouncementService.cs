﻿using SchoolConnect_DomainLayer.Models;
using static SchoolConnect_Web_App.Services.SharedClientSideServices;
using SchoolConnect_Web_App.IServices;
using System.Text;
using System.Text.Json;
using NuGet.Protocol;

namespace SchoolConnect_Web_App.Services
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly HttpClient _httpClient;
        private const string AnnouncementBasePath = "/api/Announcement";
        private Dictionary<string, object> _returnDictionary;

        public AnnouncementService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _returnDictionary = [];
        }

        public async Task<Dictionary<string, object>> CreateAnnouncementAsync(Announcement announcement)
        {
            try
            {
                StringBuilder buildString = new();
                buildString.Append("http://localhost:5293");
                buildString.Append(AnnouncementBasePath);
                buildString.Append("/Create");

                var request = new HttpRequestMessage
                {
                    Content = new StringContent(JsonSerializer.Serialize(announcement), Encoding.UTF8, "application/json"),
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

        public async Task<Dictionary<string, object>> GetAnnouncementByPrincipalIdAsync(long principalId)
        {
            try
            {
                StringBuilder buildString = new();
                buildString.Append("http://localhost:5293");
                buildString.Append(AnnouncementBasePath);
                buildString.Append("/GetAnnouncementByPrincipalId?principalId=");
                buildString.Append(principalId);

                var response = await _httpClient.GetAsync(buildString.ToString());
                return CheckSuccessStatus(response, "Announcement");
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetAllAnnBySchoolAsync(long schoolId)
        {
            try
            {
                StringBuilder buildString = new();
                buildString.Append("http://localhost:5293");
                buildString.Append(AnnouncementBasePath);
                buildString.Append("/GetAllAnnBySchool?schoolId=");
                buildString.Append(schoolId);

                var response = await _httpClient.GetAsync(buildString.ToString());
                return CheckSuccessStatus(response, "Announcement");
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> RemoveAnnouncementAsync(int announcementId)
        {
            try
            {
                StringBuilder buildString = new();
                buildString.Append("http://localhost:5293");
                buildString.Append(AnnouncementBasePath);
                buildString.Append("/Delete?announcementId=");
                buildString.Append(announcementId);

                var response = await _httpClient.DeleteAsync(buildString.ToString());
                return CheckSuccessStatus(response, "NoNeed");
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetAnnouncementById(int announcementId)
        {
            try
            {
                StringBuilder buildString = new();
                buildString.Append("http://localhost:5293");
                buildString.Append(AnnouncementBasePath);
                buildString.Append("/GetAnnouncementById?annId=");
                buildString.Append(announcementId);

                var response = await _httpClient.GetAsync(buildString.ToString());
                return CheckSuccessStatus(response, "Announcement");
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetAnnouncementByTeacherIdAsync(long teacherId)
        {
            try
            {
                StringBuilder buildString = new();
                buildString.Append("http://localhost:5293");
                buildString.Append(AnnouncementBasePath);
                buildString.Append("/GetAnnouncementsByTeacherId?id=");
                buildString.Append(teacherId);

                var response = await _httpClient.GetAsync(buildString.ToString());
                return CheckSuccessStatus(response, "Announcement");
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> UpdateAnnouncementAsync(Announcement announcement)
        {
            try
            {
                StringBuilder buildString = new();
                buildString.Append("http://localhost:5293");
                buildString.Append(AnnouncementBasePath);
                buildString.Append("/Update");

                var request = new HttpRequestMessage()
                {
                    Content = new StringContent(JsonSerializer.Serialize(announcement), Encoding.UTF8, "application/json"),
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

        public async Task<Dictionary<string, object>> GetAnnouncementsByParentIdAsync(long parentId)
        {
            try
            {
                StringBuilder buildString = new();
                buildString.Append("http://localhost:5293");
                buildString.Append(AnnouncementBasePath);
                buildString.Append("/GetAnnouncementsByParentId?parentId=");
                buildString.Append(parentId);

                var response = await _httpClient.GetAsync(buildString.ToString());
                return CheckSuccessStatus(response, "Announcement");
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
