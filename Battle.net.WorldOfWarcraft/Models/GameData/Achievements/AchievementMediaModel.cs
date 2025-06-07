using System.Text.Json.Serialization;

namespace Battle.net.WorldOfWarcraft.Models;

public class AchievementMediaModel : MediaDocument
{
    public List<Asset> Assets { get; set; }
    public int Id { get; set; }
}

public class Asset
{
    public string Key { get; set; }
    public string Value { get; set; }
    [JsonPropertyName("file_data_id")]
    public int FileDataId { get; set; }
}