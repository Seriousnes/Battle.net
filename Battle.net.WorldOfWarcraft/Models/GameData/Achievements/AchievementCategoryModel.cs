using System.Text.Json.Serialization;

namespace Battle.net.WorldOfWarcraft.Models;

public class AchievementCategoryModel : MediaDocument
{
    public List<Achievement> Achievements { get; set; }
    public List<Subcategory> Subcategories { get; set; }
    [JsonPropertyName("is_guild_category")]
    public bool IsGuildCategory { get; set; }
    [JsonPropertyName("aggregates_by_faction")]
    public AggregatesByFaction AggregatesByFaction { get; set; }
    [JsonPropertyName("display_order")]
    public int DisplayOrder { get; set; }
}

public class AchievementIndexModel : MediaDocument
{
    public List<Achievement> Achievements { get; set; }
}