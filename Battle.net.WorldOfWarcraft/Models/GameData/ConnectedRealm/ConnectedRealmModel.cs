using System.Text.Json.Serialization;

namespace Battle.net.WorldOfWarcraft.Models;

public class ConnectedRealmModel : MediaDocument
{
    public int Id { get; set; }
    [JsonPropertyName("has_queue")]
    public bool HasQueue { get; set; }
    public Status Status { get; set; }
    public Population Population { get; set; }
    public Realm[] Realms { get; set; }
    [JsonPropertyName("mythic_leaderboards")]
    public Key MythicLeaderboards { get; set; }
    public Key Auctions { get; set; }
}

public class Status
{
    public string Type { get; set; }
    public string Name { get; set; }
}

public class Population
{
    public string Type { get; set; }
    public string Name { get; set; }
}

public class Realm
{
    public int Id { get; set; }
    public RealmRegion Region { get; set; }
    [JsonPropertyName("connected_realm")]
    public Key ConnectedRealm { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public string Locale { get; set; }
    public string Timezone { get; set; }
    public RealmType Type { get; set; }
    [JsonPropertyName("is_tournament")]
    public bool IsTournament { get; set; }
    public string Slug { get; set; }
}

public class RealmRegion
{
    public Key Key { get; set; }
    public int Id { get; set; }
    public string Name { get; set; }
    public string Tag { get; set; }
}

public class RealmType
{
    public string Type { get; set; }
    public string Name { get; set; }
}