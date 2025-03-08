using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace RAG.AI.Infrastructure.Extentions;

public static class ExtensionHelper
{

    public static T ToPayloadValue<T>(this string json)
    {
        JObject obj = JObject.Parse(json);
        string value = obj.Properties().First().Value.ToString();
        return (T)Convert.ChangeType(value, typeof(T));
    }

    //public static UserType? GetUserType(this ClaimsPrincipal claimsPrincipal)
    //{
    //    Claim? result = claimsPrincipal?.FindFirst("UserType");
    //    if (result is null)
    //    {
    //        return null;
    //    }

    //    var userType = UserType.From(int.Parse(result.Value));
    //    return result != null ? userType : null;
    //}
    public static IQueryable<T>? OrderBExtensiony<T>(this IQueryable<T> query, string orderByExpression)
    {
        if (string.IsNullOrEmpty(orderByExpression))
        {
            return query;
        }

        string propertyName, orderByMethod;
        string[] strs = orderByExpression.Split(' ');
        propertyName = strs[0];

        orderByMethod = strs.Length == 1 ? "OrderBy" : strs[1].Equals("des", StringComparison.OrdinalIgnoreCase) ? "OrderByDescending" : "OrderBy";

        ParameterExpression pe = Expression.Parameter(query.ElementType);
        MemberExpression me = Expression.Property(pe, propertyName);

        MethodCallExpression orderByCall = Expression.Call(typeof(Queryable), orderByMethod, new Type[] { query.ElementType, me.Type }, query.Expression
            , Expression.Quote(Expression.Lambda(me, pe)));

        return query.Provider.CreateQuery(orderByCall) as IQueryable<T>;
    }


    public static int GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        Claim? result = claimsPrincipal?.FindFirst("UserId");
        return result == null ? default : Convert.ToInt32(result.Value);
    }


    public static string ToBase64(this byte[] data)
    {
        return data == null ? "" : Convert.ToBase64String(data);
    }

    public static string ToJson(this object? data, string title = "")
    {
        object? result = data;
        if (data == null)
        {
            result = "";
        }

        return JsonConvert.SerializeObject(new
        {
            title,
            result
        });
    }

    //public static string ToJsonPrettier(this object? data)
    //{
    //    if (data is not string result || string.IsNullOrEmpty(result) || string.IsNullOrWhiteSpace(result))
    //    {
    //        return "-";
    //    }

    //    dynamic array = JsonConvert.DeserializeObject(data as string);

    //    StringBuilder sb = new("");
    //    foreach (object? item in array)
    //    {
    //        _ = sb.AppendLine($"{item.title.ToString()} : {item.value.ToString()} \n");
    //    }
    //    return sb.ToString();
    //}


    public static string ToShamsi(this DateTime? date, string format = "yyyy/MM/dd", bool includeTime = false, string nullValue = "-")
    {
        return date == null ? nullValue : date.Value.ToShamsi(format, includeTime);
    }

    public static string ToShamsi(this DateTime date, string format = "yyyy/MM/dd", bool includeTime = false)
    {

        PersianCalendar pc = new();
        string result = format
            .Replace("yyyy", pc.GetYear(date).ToString())
            .Replace("MM", pc.GetMonth(date).ToString("00"))
            .Replace("dd", pc.GetDayOfMonth(date).ToString("00"));

        if (includeTime)
        {
            result += $" {pc.GetHour(date):00}:{pc.GetMinute(date):00}";
        }

        return result;
    }

    public static string ToShamsi(this string date, string format = "yyyy/MM/dd", bool includeTime = false)
    {
        DateTime dt = Convert.ToDateTime(date);
        return dt.ToShamsi(format, includeTime);
    }

    public static int ToShamsiInt(this string date)
    {
        DateTime dateTime = date.ToGregorian();
        PersianCalendar pc = new();

        string str = $"{pc.GetYear(dateTime)}{pc.GetMonth(dateTime):00}{pc.GetDayOfMonth(dateTime):00}";
        return int.Parse(str);
    }
    public static DateTime ToGregorian(this string persianStr)
    {
        if (string.IsNullOrEmpty(persianStr))
        {
            throw new Exception("Invalid input string");
        }

        string date = persianStr.ToLatingDigit();

        Regex regex = new(@"^\d{4}/\d{2}/\d{2}$");
        if (!regex.IsMatch(date))
        {
            throw new Exception("Invalid input format");
        }

        string[] splitedDate = date.Split('/');

        if (splitedDate.Length != 3)
        {
            throw new Exception("Invalid input format");
        }

        if (!int.TryParse(splitedDate[0], out int year) ||
            !int.TryParse(splitedDate[1], out int month) ||
            !int.TryParse(splitedDate[2], out int day))
        {
            throw new Exception("Invalid input format");
        }

        if (year < 1 || month < 1 || month > 12 || day < 1 || day > DateTime.DaysInMonth(year, month))
        {
            throw new Exception("Invalid date");
        }

        PersianCalendar pc = new();
        DateTime dt = new(year, month, day, pc);

        return dt;

    }

    public static bool ToValidateDatetime(this string persianStr)
    {
        if (string.IsNullOrEmpty(persianStr))
        {
            return false;
        }

        string date = persianStr.ToLatingDigit();

        Regex regex = new(@"^\d{4}/\d{2}/\d{2}$");
        return regex.IsMatch(date);
    }

    public static TimeSpan ToGregorianTime(this string persianStr)
    {
        string convertedTime = persianStr.ToLatingDigit();
        return TimeSpan.Parse(convertedTime);

    }

    public static string? ToEnumDisplayName(this Enum enu)
    {
        DisplayAttribute attr = GetDisplayAttribute(enu);
        return attr != null ? attr.Name : enu.ToString();
    }
    private static DisplayAttribute? GetDisplayAttribute(object value)
    {
        Type type = value.GetType();
        if (!type.IsEnum)
        {
            throw new ArgumentException(string.Format("Type {0} is not an enum", type));
        }

        // Get the enum field.
        FieldInfo? field = type.GetField(value.ToString());
        return field?.GetCustomAttribute<DisplayAttribute>();
    }
    public static string ToFixedPath(this string path)
    {
        return path.Replace(" ", "-").Replace(".", " ").Replace("+", "-").Replace("/", "-");
    }
    //public static string ToSanitizePath(this string @object)
    //{
    //    var textNormalizer = new TextNormalizer(true, true, true, true, true, true, true, true, true, true, true, true, true, true);
    //    return textNormalizer.Normalize(@object);
    //}

    public static string ToLatingDigit(this string persianStr)
    {
        Dictionary<string, string> LettersDictionary = new()
        {
            ["۰"] = "0",
            ["۱"] = "1",
            ["۲"] = "2",
            ["۳"] = "3",
            ["۴"] = "4",
            ["۵"] = "5",
            ["۶"] = "6",
            ["۷"] = "7",
            ["۸"] = "8",
            ["۹"] = "9"
        };
        return LettersDictionary.Aggregate(persianStr, (current, item) =>
                     current.Replace(item.Key, item.Value));
    }

    public static string ToPersianDigit(this string latinStr)
    {
        Dictionary<string, string> LettersDictionary = new()
        {
            ["0"] = "۰",
            ["1"] = "۱",
            ["2"] = "۲",
            ["3"] = "۳",
            ["4"] = "۴",
            ["5"] = "۵",
            ["6"] = "۶",
            ["7"] = "۷",
            ["8"] = "۸",
            ["9"] = "۹"
        };
        return LettersDictionary.Aggregate(latinStr, (current, item) =>
                     current.Replace(item.Key, item.Value));
    }

    public static string ConvertToPersianText(this int? number)
    {
        if (number is null or 0)
        {
            return "صفر";
        }

        string[] yekan = { "", "یک", "دو", "سه", "چهار", "پنج", "شش", "هفت", "هشت", "نه" };
        string[] dahgan = { "", "", "بیست", "سی", "چهل", "پنجاه", "شصت", "هفتاد", "هشتاد", "نود" };
        string[] dahyek = { "ده", "یازده", "دوازده", "سیزده", "چهارده", "پانزده", "شانزده", "هفده", "هجده", "نوزده" };
        string[] sadgan = { "", "صد", "دویست", "سیصد", "چهارصد", "پانصد", "ششصد", "هفتصد", "هشتصد", "نهصد" };
        string[] basex = { "", "هزار", "میلیون", "میلیارد", "تریلیون" };

        string result = "";
        int baseIndex = 0;

        while (number > 0)
        {
            int threeDigits = (int)(number % 1000);
            if (threeDigits > 0)
            {
                string temp = "";
                int sadgan_index = threeDigits / 100;
                int dahgan_index = threeDigits % 100 / 10;
                int yekan_index = threeDigits % 10;

                if (sadgan_index > 0)
                {
                    temp += sadgan[sadgan_index] + " و ";
                }

                if (dahgan_index == 1)
                {
                    temp += dahyek[yekan_index] + " ";
                }
                else
                {
                    if (dahgan_index > 1)
                    {
                        temp += dahgan[dahgan_index] + " و ";
                    }

                    if (yekan_index > 0)
                    {
                        temp += yekan[yekan_index] + " ";
                    }
                }

                temp += basex[baseIndex] + " و ";
                result = temp + result;
            }

            number /= 1000;
            baseIndex++;
        }

        return result[..^3].Trim();
    }
}


