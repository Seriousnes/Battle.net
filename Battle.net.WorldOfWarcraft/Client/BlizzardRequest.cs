using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace Battle.net.WorldOfWarcraft;

public class BlizzardRequest(string url, HttpMethod method, Region? region = null, Namespace? @namespace = null, Locale? locale = null)
{
    public static Region DefaultRegion { get; set; } = Region.US;
    public static Namespace DefaultNamespace { get; set; } = Namespace.Static;

    public BlizzardRequest() : this(string.Empty, HttpMethod.Get)
    {
    }

    public Region Region { get; set; } = region ?? DefaultRegion;
    public Namespace Namespace { get; set; } = @namespace ?? DefaultNamespace;
    public Locale? Locale { get; set; } = locale ?? Models.Locale.en_US;
    public HttpMethod Method { get; set; } = method;
    public string Endpoint { get; set; } = url;
    public NameValueCollection QueryParameters { get; } = HttpUtility.ParseQueryString(string.Empty, Encoding.UTF8);

    public static implicit operator HttpRequestMessage(BlizzardRequest req)
    {
        if (req.Locale.HasValue) req.QueryParameters.Add("locale", $"{req.Locale}");
        if (req.QueryParameters.HasKeys() && !req.Endpoint.EndsWith('?')) req.Endpoint += "?";
        var baseUrl = new Uri(req.Region switch
        {
            Region.CN => $"https://gateway.battlenet.com.cn/",
            _ => $@"https://{req.Region}.api.blizzard.com/"
        });
        HttpRequestMessage request = new(req.Method, new Uri(baseUrl, $"{req.Endpoint}{req.QueryParameters}"));
        request.Headers.Add("Battlenet-Namespace", $"{req.Namespace}-{req.Region}".ToLower());
        return request;
    }
}