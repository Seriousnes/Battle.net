using Battle.net.WorldOfWarcraft.Models.OAuth;

namespace Battle.net.WorldOfWarcraft;

public class Config
{
    public static Config Instance { get; } = new();

    public static string ClientId { get; set; } = "34d4f494c44147d8b9b9d5fd05653873";
    public static string ClientSecret { get; set; } = "YX1Hf52nd0cXhiu1eJ8OSGitaveC8JGt";

    internal OAuthTokenResponse OAuthToken { get; set; } = new();
}