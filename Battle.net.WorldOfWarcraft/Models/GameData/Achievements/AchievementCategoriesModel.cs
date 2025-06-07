using System.Text.Json.Serialization;

namespace Battle.net.WorldOfWarcraft.Models;

public class AchievementCategoriesModel : MediaDocument
{
    public List<Category> Categories { get; set; }
    [JsonPropertyName("root_categories")]
    public List<Category> RootCategories { get; set; }
    [JsonPropertyName("guild_categories")]
    public List<Category> GuildCategories { get; set; }
}