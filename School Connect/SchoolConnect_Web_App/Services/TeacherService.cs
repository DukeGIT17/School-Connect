﻿using SchoolConnect_DomainLayer.Models;
using SchoolConnect_Web_App.IServices;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
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
                return CheckSuccessStatus(response, "NoNeed");
                
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

        public async Task<Dictionary<string, object>> GetTeachersBySchoolAsync(long schoolId)
        {
            try
            {
                StringBuilder buildString = new();
                buildString.Append("http://localhost:5293");
                buildString.Append(teacherBasePath);
                buildString.Append("/GetTeachersBySchool?schoolId=");
                buildString.Append(schoolId);

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

        public async Task<Dictionary<string, object>> GetTeacherByEmailAddressAsync(string email)
        {
            try
            {
                StringBuilder buildString = new();
                buildString.Append("http://localhost:5293");
                buildString.Append(teacherBasePath);
                buildString.Append("/GetTeacherByEmailAddress?email=");
                buildString.Append(email);

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

        public async Task<Dictionary<string, object>> UpdateClassAllocationAsync(Teacher teacher)
        {
            try
            {
                StringBuilder buildString = new();
                buildString.Append("http://localhost:5293");
                buildString.Append(teacherBasePath);
                buildString.Append("/UpdateClassAllocation");

                var formData = new MultipartFormDataContent();

                foreach (var property in typeof(Teacher).GetProperties())
                {
                    var value = property.GetValue(teacher);
                    if (value is IFormFile || value is School || value is IEnumerable<GroupTeacher> || value is IEnumerable<Announcement>)
                        continue;
                    else if (value is IEnumerable<string> subjects)
                    {
                        int index = 0;
                        foreach (var subject in subjects)
                        {
                            formData.Add(new StringContent(subject), $"{property.Name}[{index}]");
                            index++;
                        }
                        continue;
                    }
                    else if (value is not null && value is SubGrade subGrade)
                    {
                        foreach (var gradeProperty in typeof(SubGrade).GetProperties())
                        {
                            var gradeValue = gradeProperty.GetValue(subGrade);
                            if ((gradeValue is Teacher || gradeValue is Grade) && gradeValue is not string)
                                continue;

                            if (gradeValue is IEnumerable<string> subjectsTaught)
                            {
                                int index = 0;
                                foreach (var subjectTaught in subjectsTaught)
                                {
                                    formData.Add(new StringContent(subjectTaught), $"{property.Name}.{gradeProperty.Name}[{index}]");
                                    index++;
                                }
                                continue;
                            }

                            if (gradeValue is not null)
                                formData.Add(new StringContent(gradeValue.ToString()), $"{property.Name}.{gradeProperty.Name}");
                        }
                    }
                    else if (value is not null && value is IEnumerable<TeacherGrade> classes)
                    {
                        int index = 0;
                        foreach (var item in classes)
                        {
                            foreach (var itemProperty in typeof(TeacherGrade).GetProperties())
                            {
                                var itemValue = itemProperty.GetValue(item);
                                if (itemValue is Teacher)
                                    continue;
                                
                                if (itemValue is not null && itemValue is SubGrade cls)
                                {
                                    foreach (var gradeProperty in typeof(SubGrade).GetProperties())
                                    {
                                        var gradeValue = gradeProperty.GetValue(cls);
                                        if ((gradeValue is Teacher || gradeValue is Grade) && gradeValue is not string)
                                            continue;

                                        if (gradeValue is IEnumerable<string> subjectsTaught)
                                        {
                                            int count = 0;
                                            foreach (var subjectTaught in subjectsTaught)
                                            {
                                                var val = $"{property.Name}[{index}].{itemProperty.Name}.{gradeProperty.Name}[{count}]";
                                                formData.Add(new StringContent(subjectTaught), $"{property.Name}[{index}].{itemProperty.Name}.{gradeProperty.Name}[{count}]");
                                                count++;
                                            }
                                            continue;
                                        }

                                        if (gradeValue is not null)
                                        {
                                            var val = $"{property.Name}[{index}].{gradeProperty.Name}";
                                            formData.Add(new StringContent(gradeValue.ToString()), $"{property.Name}[{index}].{itemProperty.Name}.{gradeProperty.Name}");
                                        }
                                    }
                                }

                                if (itemValue is not null)
                                    formData.Add(new StringContent(itemValue.ToString()), $"{property.Name}[{index}].{itemProperty.Name}");
                            }
                            index++;
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

        public async Task<Dictionary<string, object>> GetAttendanceRecordsByTeacher(long teacherId)
        {
            try
            {
                StringBuilder buildString = new();
                buildString.Append("http://localhost:5293");
                buildString.Append(teacherBasePath);
                buildString.Append("/GetAttendanceRecordsByTeacher?teacherId=");
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

        public async Task<Dictionary<string, object>> MarkAttendanceAsync(IEnumerable<Attendance> attendanceRecords)
        {
            try
            {
                StringBuilder buildString = new();
                buildString.Append("http://localhost:5293");
                buildString.Append(teacherBasePath);
                buildString.Append("/MarkAttendance");

                var request = new HttpRequestMessage
                {
                    Content = new StringContent(JsonSerializer.Serialize(attendanceRecords), Encoding.UTF8, "application/json"),
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

        public async Task<Dictionary<string, object>> GetParentsByTeacherClassesAsync(long teacherId)
        {
            try
            {
                StringBuilder buildString = new();
                buildString.Append("http://localhost:5293");
                buildString.Append(teacherBasePath);
                buildString.Append("/GetParentsByTeacherClasses?teacherId=");
                buildString.Append(teacherId);

                var response = await _httpClient.GetAsync(buildString.ToString());
                return CheckSuccessStatus(response, "Parent");
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetGradesByTeacherAsync(long teacherId)
        {
            try
            {
                StringBuilder buildString = new();
                buildString.Append("http://localhost:5293");
                buildString.Append(teacherBasePath);
                buildString.Append("/GetGradesByTeacher?teacherId=");
                buildString.Append(teacherId);

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
    }
}
