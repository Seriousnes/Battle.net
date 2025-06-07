using Battle.net.WorldOfWarcraft.Models.AuctionHouse;

namespace Battle.net.WorldOfWarcraft.Functions.GameData;

public class ActionHouseFunction(IApiRequestExecutor api) : BaseFunction(api), IActionHouseFunction
{
    public AuctionsModel GetAuctions(int connectedRealmId)
    {
        return ApiRequest<AuctionsModel>($"/data/wow/connected-realm/{connectedRealmId}/auctions", @namespace: Namespace.Dynamic);
    }
}

public interface IActionHouseFunction
{
    AuctionsModel GetAuctions(int connectedRealmId);
}