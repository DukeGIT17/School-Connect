using SchoolConnect_DomainLayer.Models;
using System.Text.Json;

namespace SchoolConnect_Web_App.Services
{
    public static class SharedClientSideServices
    {
        public static Dictionary<string, object> CheckSuccessStatus(HttpResponseMessage response)
        {
            Dictionary<string, object> _returnDictionary = [];
            try
            {
                if (!response.IsSuccessStatusCode)
                {
                    _ = string.IsNullOrEmpty(response.Content.ReadAsStringAsync().Result) ? throw new("Response from the API was null or empty.") : "";
                    throw new(response.Content.ReadAsStringAsync().Result ?? "Operation failed; reason not provided.");
                }

                var result = response.Content.ReadFromJsonAsync<Dictionary<string, object>>().Result!;
                _returnDictionary = ConvertJsonElements(result);

                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                var dict = _returnDictionary["Result"] as Dictionary<string, object>;
                var school = dict!["sysAdminSchoolNP"] as Dictionary<string, object>;

                SysAdmin admin = new()
                {
                    Id = (long)dict!["id"],
                    ProfileImage = dict["profileImage"].ToString(),
                    Name = dict["name"].ToString()!,
                    Surname = dict["surname"].ToString()!,
                    Gender = dict["gender"].ToString()!,
                    Role = dict["role"].ToString()!,
                    StaffNr = (long)dict!["staffNr"],
                    EmailAddress = dict["emailAddress"].ToString()!,
                    PhoneNumber = (long)dict["phoneNumber"],
                    SysAdminSchoolNP = school is not null ? new()
                    {
                        Id = (long)school!["id"],
                        EmisNumber = (long)school!["emisNumber"],
                        Logo = school["logo"].ToString(),
                        Name = school["name"].ToString()!,
                        DateRegistered = Convert.ToDateTime(school["dateRegistered"]),
                        Type = school["type"].ToString()!,
                        SystemAdminId = (long)school["systemAdminId"],
                    }
                    : null,
                };

                _returnDictionary["Result"] = admin;
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
