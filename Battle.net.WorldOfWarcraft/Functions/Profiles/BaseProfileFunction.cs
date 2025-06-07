namespace Battle.net.WorldOfWarcraft.Functions.Profiles;
public abstract class BaseProfileFunction(IApiRequestExecutor api) : BaseFunction(api)
{
    protected override T ApiRequest<T>(string url, Region region = Region.US, Namespace @namespace = Namespace.Profile, Locale? locale = null)
    {
        return base.ApiRequest<T>(url, region, @namespace, locale);
    }

    protected override async Task<T> ApiRequestAsync<T>(string url, Region region = Region.US, Namespace @namespace = Namespace.Profile, Locale? locale = null)
    {
        return await base.ApiRequestAsync<T>(url, region, @namespace, locale);
    }
}