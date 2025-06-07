using System.Collections.Specialized;
using System.ComponentModel;

namespace Battle.net.WorldOfWarcraft.Models;

public enum Locale
{
    [Description("English (United States)")]
    en_US,
    [Description("Spanish (Mexico)")]
    es_MX,
    [Description("Portuguese")]
    pt_BR,
    [Description("German")]
    de_DE,
    [Description("English (Great Britain)")]
    en_GB,
    [Description("Spanish (Spain)")]
    es_ES,
    [Description("French")]
    fr_FR,
    [Description("Italian")]
    it_IT,
    [Description("Russian")]
    ru_RU,
    [Description("Korean")]
    ko_KR,
    [Description("Chinese (Traditional)")]
    zh_TW,
    [Description("Chinese (Simplified)")]
    zh_CN
}

public static class LocaleExtensions
{
    public static void Add(this NameValueCollection queryParams, Locale locale)
    {
        queryParams.Add("locale", $"{locale}");
    }
}