namespace Battle.net.WorldOfWarcraft.Functions;

public abstract class BaseFunction(IApiRequestExecutor api) : IBaseFunction
{
    protected virtual T ApiRequest<T>(string url, Region region = Region.US, Namespace @namespace = Namespace.Static, Locale? locale = null) where T : new()
    {
        return ApiRequestAsync<T>(url, region, @namespace, locale).Result;
    }

    protected virtual async Task<T> ApiRequestAsync<T>(string url, Region region = Region.US, Namespace @namespace = Namespace.Static, Locale? locale = null) where T : new()
    {
        var result = await api.ExecuteAsync<T>(new BlizzardRequest(url, HttpMethod.Get, region, @namespace, locale));
        return result is null ? throw new HttpRequestException("Response is null") : result;
    }
}

public interface IBaseFunction
{
}