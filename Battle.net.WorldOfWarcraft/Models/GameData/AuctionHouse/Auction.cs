using System.Text.Json.Serialization;

namespace Battle.net.WorldOfWarcraft.Models.AuctionHouse;

public class Auction
{
    [JsonPropertyName("id")]
    public int AuctionId { get; set; }
    public Item Item { get; set; }
    public long Buyout { get; set; }
    public int Quantity { get; set; }
    [JsonPropertyName("Time_left")]
    public string TimeLeft { get; set; }
    public long Bid { get; set; }
}