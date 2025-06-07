using Battle.net.WorldOfWarcraft.Models;
using Battle.net.WorldOfWarcraft.Models.OAuth;

namespace Battle.net.WorldOfWarcraft.Functions.GameData;

public class ConnectedRealms(IApiRequestExecutor api) : BaseFunction(api), IConnectedRealms
{
    public ConnectedRealmsModel GetConnectedRealms()
    {
        return base.ApiRequest<ConnectedRealmsModel>("/data/wow/connected-realm/index", @namespace: Namespace.Dynamic);
    }

    public ConnectedRealmModel GetConnectedRealm(int connectedRealmId)
    {
        return ApiRequest<ConnectedRealmModel>($"/data/wow/connected-realm/{connectedRealmId}", @namespace: Namespace.Dynamic);
    }

    public ConnectedRealmSearchModel SearchConnectedRealms(string? statusType = null, string? realmTimezone = null, string? orderBy = null, int? page = null)
    {
        var queryParams = new List<string>();

        if (!string.IsNullOrEmpty(statusType))
            queryParams.Add($"status.type={statusType}");

        if (!string.IsNullOrEmpty(realmTimezone))
            queryParams.Add($"realms.timezone={realmTimezone}");

        if (!string.IsNullOrEmpty(orderBy))
            queryParams.Add($"orderby={orderBy}");

        if (page.HasValue)
            queryParams.Add($"_page={page.Value}");

        var queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";

        return ApiRequest<ConnectedRealmSearchModel>($"/data/wow/search/connected-realm{queryString}", @namespace: Namespace.Dynamic);
    }
}

public interface IConnectedRealms
{
    ConnectedRealmsModel GetConnectedRealms();
    ConnectedRealmModel GetConnectedRealm(int connectedRealmId);
    ConnectedRealmSearchModel SearchConnectedRealms(string? statusType = null, string? realmTimezone = null, string? orderBy = null, int? page = null);
}