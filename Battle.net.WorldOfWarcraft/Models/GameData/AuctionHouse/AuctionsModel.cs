using System.Text.Json.Serialization;

namespace Battle.net.WorldOfWarcraft.Models.AuctionHouse;

public class AuctionsModel : MediaDocument
{
    [JsonPropertyName("connected_realm")]
    public ConnectedRealmsModel ConnectedRealm { get; set; }
    public List<Auction> Auctions { get; set; }
    public Commodities Commodities { get; set; }
}