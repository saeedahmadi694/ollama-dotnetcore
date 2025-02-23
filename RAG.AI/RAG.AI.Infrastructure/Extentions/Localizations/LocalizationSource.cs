using System.Reflection;
using System.Xml.Linq;

namespace RAG.AI.Infrastructure.Extentions.Localizations;

public static class LocalizationSource
{
    private static readonly Dictionary<string, string> _localizations;

    static LocalizationSource()
    {
        var fileName = "RAG.AI.Infrastructure.Extentions.Localizations.Localization.xml";
        _localizations = new Dictionary<string, string>();
        var assembly = typeof(LocalizationSource).GetTypeInfo().Assembly;
        var stream = assembly.GetManifestResourceStream(fileName);
        if (stream != null)
        {
            var xml = XDocument.Load(stream);
            _localizations = xml.Descendants("text")
                                .ToDictionary(k => k.Attribute("name").Value, v => v.Value);
        }
    }

    public static string L(string key)
    {
        _localizations.TryGetValue(key, out var value);
        return value ?? key;
    }

    public static string L(string key, params string[] args)
    {
        _localizations.TryGetValue(key, out var value);
        return !string.IsNullOrEmpty(value) ? string.Format(value, args) : key;
    }

    //public static List<KeyValueDto> AllLocalizations()
    //{
    //    return _localizations.Select(e => new KeyValueDto(e.Key, e.Value)).ToList();
    //}

    public static Dictionary<string, string> GetAll()
    {
        return _localizations;
    }
}







