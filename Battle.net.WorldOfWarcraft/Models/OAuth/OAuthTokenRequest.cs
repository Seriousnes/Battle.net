using System.Text.Json.Serialization;

namespace Battle.net.WorldOfWarcraft.Models.OAuth;

internal abstract class OAuthTokenRequest
{
    [JsonPropertyName("grant_type")]
    public abstract string GrantType { get; }
    [JsonPropertyName("client_id")]
    public string ClientId { get; set; }
    [JsonPropertyName("client_secret")]
    public string ClientSecret { get; set; }
}

internal class OAuth2RefreshTokenRequest : OAuthTokenRequest
{
    public override string GrantType => "refresh_token";
    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; }
}