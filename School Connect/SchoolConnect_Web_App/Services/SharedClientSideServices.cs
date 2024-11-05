using SchoolConnect_DomainLayer.Models;
using System.Collections;
using System.Reflection;
using System.Text.Json;

namespace SchoolConnect_Web_App.Services
{
    public static class SharedClientSideServices
    {
        public static Dictionary<string, object> CheckSuccessStatus(HttpResponseMessage response, string convertTo)
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

                if (convertTo != "NoNeed")
                {
                    var dict = _returnDictionary["Result"] as Dictionary<string, object>;
                    var list = _returnDictionary["Result"] as List<object>;

                    SysAdmin admin = new();
                    Principal principal = new();
                    Parent parent = new();
                    School school = new();
                    Announcement ann = new();
                    Learner learner = new();
                    Teacher teacher = new();
                    SubGrade subGrade = new();
                    Grade grade = new();
                    Attendance attendance = new();

                    switch (convertTo)
                    {
                        case "Admin":
                            AssignValuesFromDictionary(admin, dict!);
                            _returnDictionary["Result"] = admin;
                            break;

                        case "School":
                            AssignValuesFromDictionary(school, dict!);
                            _returnDictionary["Result"] = school;
                            break;

                        case "Principal":
                            AssignValuesFromDictionary(principal, dict!);
                            _returnDictionary["Result"] = principal;
                            break;

                        case "Teacher":
                            if (dict is null)
                            {
                                List<Teacher> teachers = [];
                                foreach (var val in list)
                                {
                                    AssignValuesFromDictionary(teacher, val as Dictionary<string, object>);
                                    var teacherString = JsonSerializer.Serialize(teacher);
                                    teachers.Add(JsonSerializer.Deserialize<Teacher>(teacherString)!);
                                }
                                _returnDictionary["Result"] = teachers;
                            }
                            else
                            {
                                AssignValuesFromDictionary(teacher, dict!);
                                _returnDictionary["Result"] = teacher;
                            }
                            break;

                        case "Announcement":
                            if (dict is null)
                            {
                                List<Announcement> anns = [];

                                foreach (var val in list)
                                {
                                    AssignValuesFromDictionary(ann, val as Dictionary<string, object>);
                                    var annString = JsonSerializer.Serialize(ann);
                                    anns.Add(JsonSerializer.Deserialize<Announcement>(annString)!);
                                }
                                _returnDictionary["Result"] = anns;
                            }
                            else
                            {
                                AssignValuesFromDictionary(ann, dict!);
                                _returnDictionary["Result"] = ann;
                            }
                            break;

                        case "Learner":
                            if (dict is null)
                            {
                                List<Learner> learners = [];
                                foreach (var val in list)
                                {
                                    AssignValuesFromDictionary(learner, val as Dictionary<string, object>);
                                    var learnerString = JsonSerializer.Serialize(learner);
                                    learners.Add(JsonSerializer.Deserialize<Learner>(learnerString)!);
                                }
                                _returnDictionary["Result"] = learners;
                            }
                            else
                            {
                                AssignValuesFromDictionary(learner, dict!);
                                _returnDictionary["Result"] = learner;
                            }
                            break;

                        case "SubGrade":
                            if (dict is null)
                            {
                                List<SubGrade> subGrades = [];
                                foreach (var val in list)
                                {
                                    AssignValuesFromDictionary(subGrade, val as Dictionary<string, object>);
                                    var subGradeString = JsonSerializer.Serialize(subGrade);
                                    subGrades.Add(JsonSerializer.Deserialize<SubGrade>(subGradeString)!);
                                }
                                _returnDictionary["Result"] = subGrades;
                            }
                            else
                            {
                                AssignValuesFromDictionary(subGrade, dict!);
                                _returnDictionary["Result"] = subGrade;
                            }
                            break;
                        
                        case "Grade":
                            if (dict is null)
                            {
                                List<Grade> grades = [];
                                foreach (var val in list)
                                {
                                    AssignValuesFromDictionary(grade, val as Dictionary<string, object>);
                                    var gradeString = JsonSerializer.Serialize(grade);
                                    grades.Add(JsonSerializer.Deserialize<Grade>(gradeString)!);
                                }
                                _returnDictionary["Result"] = grades;
                            }
                            else
                            {
                                AssignValuesFromDictionary(grade, dict!);
                                _returnDictionary["Result"] = grade;
                            }
                            break;

                        case "Attendance":
                            if (dict is null)
                            {
                                List<Attendance> attendanceRecords = [];
                                foreach (var val in list)
                                {
                                    AssignValuesFromDictionary(attendance, val as Dictionary<string, object>);
                                    var attRecString = JsonSerializer.Serialize(attendance);
                                    attendanceRecords.Add(JsonSerializer.Deserialize<Attendance>(attRecString)!);
                                }
                                _returnDictionary["Result"] = attendanceRecords;
                            }
                            else
                            {
                                AssignValuesFromDictionary(attendance, dict!);
                                _returnDictionary["Result"] = attendance;
                            }
                            break;

                        case "Parent":
                            if (dict is null)
                            {
                                List<Parent> parents = [];
                                foreach (var val in list)
                                {
                                    AssignValuesFromDictionary(parent, val as Dictionary<string, object>);
                                    var parentString = JsonSerializer.Serialize(parent);
                                    parents.Add(JsonSerializer.Deserialize<Parent>(parentString)!);
                                }
                                _returnDictionary["Result"] = parents;
                            }
                            else
                            {
                                AssignValuesFromDictionary(parent, dict!);
                                _returnDictionary["Result"] = parent;
                            }
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

        public static void AssignValuesFromDictionary<T>(T obj, Dictionary<string, object> dict)
        {
            Type type = obj.GetType();

            foreach (var kvp in dict)
             {
                var key = string.Concat(kvp.Key.First().ToString().ToUpper(), kvp.Key.AsSpan(1));
                PropertyInfo propertyInfo = type.GetProperty(key);


                if (propertyInfo is not null && propertyInfo.CanWrite)
                {
                    if (kvp.Value is Dictionary<string, object> nestedDictionary)
                    {
                        var nestedObj = Activator.CreateInstance(propertyInfo.PropertyType);
                        AssignValuesFromDictionary(nestedObj, nestedDictionary);
                        propertyInfo.SetValue(obj, nestedObj);
                    }
                    else if (propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType != typeof(long) && typeof(IList<>).IsAssignableFrom(propertyInfo.PropertyType.GetGenericTypeDefinition()))
                    {
                        var elementType = propertyInfo.PropertyType.GetGenericArguments()[0];

                        if (kvp.Value is IEnumerable<object> listValues)
                        {

                            if (elementType == typeof(string))
                            {
                                var lType = typeof(List<>).MakeGenericType(elementType);
                                var l = (IList)Activator.CreateInstance(lType)!;
                                foreach (var item in listValues)
                                {
                                    var convertedItem = Convert.ChangeType(item, elementType);
                                    l.Add(convertedItem);
                                }
                                propertyInfo.SetValue(obj, l);
                                continue;
                            }

                            var listType = typeof(List<>).MakeGenericType(elementType);
                            var list = (IList)Activator.CreateInstance(listType)!;

                            foreach (var item in listValues)
                            {
                                var typeToUse = Activator.CreateInstance(elementType);
                                AssignValuesFromDictionary(typeToUse, item as Dictionary<string, object>);
                                list.Add(typeToUse);
                            }
                            propertyInfo.SetValue(obj, list);
                        }
                    }
                    else
                    {
                        object valueToSet;
                        if (Nullable.GetUnderlyingType(propertyInfo.PropertyType) is not null)
                            valueToSet = kvp.Value is null ? null : Convert.ChangeType(kvp.Value, Nullable.GetUnderlyingType(propertyInfo.PropertyType));
                        else
                            valueToSet = Convert.ChangeType(kvp.Value, propertyInfo.PropertyType);
                        propertyInfo.SetValue(obj, valueToSet);
                    }
                }
            }
        }

        public static object ConvertJsonElement(JsonElement element)
        {
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
