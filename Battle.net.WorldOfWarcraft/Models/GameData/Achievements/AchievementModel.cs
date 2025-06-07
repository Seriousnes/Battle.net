using System.Text.Json.Serialization;

namespace Battle.net.WorldOfWarcraft.Models;

public class AchievementModel : MediaDocument
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Category Category { get; set; }
    public int Points { get; set; }
    [JsonPropertyName("is_account_wide")]
    public bool IsAccountWide { get; set; }
    public Criteria Criteria { get; set; }
    [JsonPropertyName("next_achievement")]
    public Achievement NextAchievement { get; set; }
    public Media Media { get; set; }
    [JsonPropertyName("display_order")]
    public int DisplayOrder { get; set; }
}