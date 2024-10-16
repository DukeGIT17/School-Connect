using SchoolConnect_DomainLayer.Models;
using System.Text.Json;

namespace SchoolConnect_Web_App.Services
{
    public static class SharedClientSideServices
    {
        public static Dictionary<string, object> CheckSuccessStatus(HttpResponseMessage response, string sourceMethod)
        {
            Dictionary<string, object> _returnDictionary = [];
            try
            {
                if (!response.IsSuccessStatusCode)
                {
                    _ = string.IsNullOrEmpty(response.Content.ReadAsStringAsync().Result) ? throw new(response.ReasonPhrase) : "";
                    throw new(response.Content.ReadAsStringAsync().Result ?? "Operation failed; reason not provided.");
                }

                var result = response.Content.ReadFromJsonAsync<Dictionary<string, object>>().Result!;
                _returnDictionary = ConvertJsonElements(result);

                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                if (sourceMethod != "NoNeed")
                {
                    var dict = _returnDictionary["Result"] as Dictionary<string, object>;

                    switch (sourceMethod)
                    {
                        case nameof(SystemAdminService.GetAdminByIdAsync):
                        case nameof(SystemAdminService.GetAdminByStaffNr):
                        
                            var schoolDict = dict!["sysAdminSchoolNP"] as Dictionary<string, object>;

                            SysAdmin admin = new()
                            {
                                Id = (long)dict!["id"],
                                ProfileImage = dict["profileImage"].ToString(),
                                Title = dict["title"].ToString()!,
                                Name = dict["name"].ToString()!,
                                Surname = dict["surname"].ToString()!,
                                Gender = dict["gender"].ToString()!,
                                Role = dict["role"].ToString()!,
                                StaffNr = dict!["staffNr"].ToString()!,
                                EmailAddress = dict["emailAddress"].ToString()!,
                                PhoneNumber = (long)dict["phoneNumber"],
                                SysAdminSchoolNP = schoolDict is not null ? new()
                                {
                                    Id = (long)schoolDict!["id"],
                                    EmisNumber = schoolDict!["emisNumber"].ToString()!,
                                    Logo = schoolDict["logo"].ToString(),
                                    Name = schoolDict["name"].ToString()!,
                                    DateRegistered = Convert.ToDateTime(schoolDict["dateRegistered"]),
                                    Type = schoolDict["type"].ToString()!,
                                    SystemAdminId = (long)schoolDict["systemAdminId"],
                                }
                                : null,
                            };

                            _returnDictionary["Result"] = admin;
                            break;

                        case nameof(SchoolService.GetSchoolByAdminAsync):
                        case nameof(SchoolService.GetSchoolByIdAsync):
                        case nameof(SchoolService.GetSchoolByLearnerIdNoAsync):
                             var address = dict!["schoolAddress"] as Dictionary<string, object>;
                            School school = new()
                            {
                                Id = (long)dict!["id"],
                                EmisNumber = dict["emisNumber"].ToString()!,
                                Logo = dict["logo"].ToString(),
                                Name = dict["name"].ToString()!,
                                DateRegistered = Convert.ToDateTime(dict["dateRegistered"]),
                                Type = dict["type"].ToString()!,
                                TelePhoneNumber = (long)dict["telePhoneNumber"],
                                EmailAddress = dict["emailAddress"].ToString()!,
                                SchoolAddress = new()
                                {
                                    AddressID = Convert.ToInt32(address!["addressID"]),
                                    Street = address["street"].ToString()!,
                                    Suburb = address["suburb"].ToString()!,
                                    City = address["city"].ToString()!,
                                    PostalCode = address["postalCode"].ToString()!,
                                    Province = address["province"].ToString()!,
                                    SchoolID = (long)address["schoolID"]
                                }
                            };

                            _returnDictionary["Result"] = school;
                            break;

                        case nameof(PrincipalService.GetPrincipalByIdAsync):
                            var princSchool = dict!["principalSchoolNP"] as Dictionary<string, object>;
                            var princSchoolAddress = princSchool!["schoolAddress"] as Dictionary<string, object>;
                            Principal principal = new()
                            {
                                Id = (long)dict!["id"],
                                Title = dict["title"].ToString()!,
                                Name = dict["name"].ToString()!,
                                Surname = dict["surname"].ToString()!,
                                Gender = dict["gender"].ToString()!,
                                Role = dict["role"].ToString()!,
                                StaffNr = dict["staffNr"].ToString(),
                                EmailAddress = dict["emailAddress"].ToString()!,
                                PhoneNumber = (long)dict["phoneNumber"],
                                SchoolID = (long)dict["schoolID"],
                                PrincipalSchoolNP = new()
                                {
                                    Id = (long)princSchool!["id"],
                                    EmisNumber = princSchool["emisNumber"].ToString()!,
                                    Logo = princSchool["logo"].ToString(),
                                    Name = princSchool["name"].ToString()!,
                                    DateRegistered = Convert.ToDateTime(princSchool["dateRegistered"]),
                                    Type = princSchool["type"].ToString()!,
                                    TelePhoneNumber = (long)princSchool["telePhoneNumber"],
                                    EmailAddress = princSchool["emailAddress"].ToString()!,
                                    SystemAdminId = (long)princSchool["systemAdminId"],
                                    SchoolAddress = new()
                                    {
                                        AddressID = Convert.ToInt32(princSchoolAddress!["addressID"]),
                                        Street = princSchoolAddress["street"].ToString()!,
                                        Suburb = princSchoolAddress["suburb"].ToString()!,
                                        City = princSchoolAddress["city"].ToString()!,
                                        PostalCode = princSchoolAddress["postalCode"].ToString()!,
                                        Province = princSchoolAddress["province"].ToString()!,
                                    }
                                }
                            };

                            _returnDictionary["Result"] = principal;
                            break;

                        case nameof(AnnouncementService.GetAnnouncementByPrincipalIdAsync):
                            Announcement announcement = new()
                            {
                                AnnouncementId = Convert.ToInt32(dict!["announcementId"]),
                                Title = dict!["title"].ToString()!,
                                Recipients = dict["recipients"] as List<string>,
                                Content = dict["content"].ToString()!,
                                SendEmail = (bool)dict["sendEmail"],
                                SendSMS = (bool)dict["sendSMS"],
                                ScheduleForLater = (bool)dict["scheduleForLater"],
                                DateCreated = Convert.ToDateTime(dict["dateCreated"]),
                                TimeToPost = Convert.ToDateTime(dict["timeToPost"]),
                                TeacherID = dict["teacherID"] is not null ? (long)dict["teacherID"] : null,
                                PrincipalID = dict["principalID"] is not null ? (long)dict["principalID"] : null,
                                SchoolID = (long)dict["schoolID"]
                            };

                            _returnDictionary["Result"] = announcement;
                            break;

                        case nameof(LearnerService.GetLearnerByIdNo):
                            var sDict = dict!["schoolLearnerNP"] as Dictionary<string, object>;
                            var aDict = dict["schoolAddress"] as Dictionary<string, object>;
                            Learner learner = new()
                            {
                                Id = Convert.ToInt64(dict!["id"]),
                                ProfileImage = dict["profileImage"].ToString(),
                                ProfileImageFile = dict["profileImageFile"] as IFormFile,
                                Title = dict["title"].ToString(),
                                Name = dict["name"].ToString(),
                                Surname = dict["surname"].ToString(),
                                Gender = dict["gender"].ToString(),
                                Role = dict["role"].ToString(),
                                IdNo = dict["idNo"].ToString()!,
                                ClassCode = dict["classCode"].ToString()!,
                                Subjects = dict["subjects"] as List<string>,
                                SchoolID = Convert.ToInt64(dict["schoolID"]),
                                ClassID = Convert.ToInt32(dict["classID"]),
                                LearnerSchoolNP = new()
                                {
                                    Id = Convert.ToInt64(sDict!["id"]),
                                    EmisNumber = sDict["emisNumber"].ToString()!,
                                    Logo = sDict["logo"].ToString(),
                                    SchoolLogoFile = sDict["schoolLogoFile"] as IFormFile,
                                    Name = sDict["name"].ToString()!,
                                    DateRegistered = Convert.ToDateTime(sDict["dateRegistered"]),
                                    Type = sDict["type"].ToString()!,
                                    TelePhoneNumber = Convert.ToInt64(sDict["telePhoneNumber"]),
                                    EmailAddress = sDict["emailAddress"].ToString()!,
                                    SystemAdminId = Convert.ToInt64(sDict["systemAdminId"]),
                                    SchoolAddress = new()
                                    {
                                        AddressID = Convert.ToInt32(aDict!["addressID"]),
                                        Street = aDict["street"].ToString()!,
                                        Suburb = aDict["suburb"].ToString()!,
                                        City = aDict["city"].ToString()!,
                                        PostalCode = aDict["postalCode"].ToString()!,
                                        Province = aDict["province"].ToString()!,
                                        SchoolID = Convert.ToInt64(aDict["schoolID"])
                                    }
                                }
                            };

                            _returnDictionary["Result"] = learner;
                            break;

                        default: throw new("Something went wrong");
                    }
                }

                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public static object ConvertJsonElement(JsonElement element)
        {
            SysAdmin admin = new SysAdmin();
            switch (element.ValueKind)
            {
                case JsonValueKind.Object:
                    var dict = new Dictionary<string, object>();
                    foreach (var property in element.EnumerateObject())
                        dict[property.Name] = ConvertJsonElement(property.Value);
                    return dict;

                case JsonValueKind.Array:
                    var list = new List<object>();
                    foreach (var item in element.EnumerateArray())
                        list.Add(ConvertJsonElement(item));
                    return list;

                case JsonValueKind.String:
                    return element.GetString();

                case JsonValueKind.Number:
                    if (element.TryGetInt64(out long longValue))
                        return longValue;
                    if (element.TryGetDouble(out double doubleValue))
                        return doubleValue;
                    return element.GetDecimal();

                case JsonValueKind.True:
                case JsonValueKind.False:
                    return element.GetBoolean();

                case JsonValueKind.Null:
                    return null;

                default:
                    return element.GetRawText();
            }
        }

        public static Dictionary<string, object> ConvertJsonElements(Dictionary<string, object> dictionary)
        {
            var result = new Dictionary<string, object>();
            foreach (var keyValue in dictionary)
            {
                if (keyValue.Value is JsonElement element)
                    result[keyValue.Key] = ConvertJsonElement(element);
                else
                    result[keyValue.Key] = keyValue.Value;
            }
            return result;
        }
    }
}
