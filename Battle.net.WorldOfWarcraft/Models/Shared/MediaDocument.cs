using System.Text.Json.Serialization;

namespace Battle.net.WorldOfWarcraft.Models;

public class MediaDocument
{
    [JsonPropertyName("_links")]
    public Links Links { get; set; }
}

public class Links
{
    public Self Self { get; set; }
}

public class Self
{
    public string Href { get; set; }
}

public class Media : KeyModel
{
    public int Id { get; set; }
    public string Name { get; set; }
}