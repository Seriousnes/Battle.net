using System.Collections.Specialized;

namespace Battle.net.WorldOfWarcraft.Models;

public enum Namespace
{
    Static,
    Dynamic,
    Profile
}

public static class NamespaceExtensions
{
    public static void Add(this NameValueCollection queryParams, Namespace @namespace, Region region)
    {
        queryParams.Add("namespace", $"{@namespace}-{region}".ToLower());
    }
}