using System.Text.Json.Serialization;

namespace Battle.net.WorldOfWarcraft.Models.OAuth;

public class OAuthTokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }
    [JsonPropertyName("expires_in")]
    public long ExpiresIn { get; set; }
    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; set; }
    public string? Scope { get; set; }
    [JsonIgnore]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    [JsonIgnore]
    public DateTime Expiry => this.CreatedAt.AddSeconds(this.ExpiresIn);

    public bool IsExpiredOrExpiring()
    {
        return (this.Expiry - DateTime.Now).TotalSeconds <= 30;
    }
}