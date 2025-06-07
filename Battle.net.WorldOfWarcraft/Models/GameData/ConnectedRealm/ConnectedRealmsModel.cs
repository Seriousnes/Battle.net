using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Battle.net.WorldOfWarcraft.Models;

public class ConnectedRealmsModel : MediaDocument
{
    [JsonPropertyName("connected_realms")]
    public ConnectedRealmSummary[] ConnectedRealms { get; set; }
}

[DebuggerDisplay("{Id}")]
public partial class ConnectedRealmSummary : Key
{
    [GeneratedRegex(@"connected-realm/(\d+)")]
    private static partial Regex ConnectedRealmIdPattern();

    public int Id => ConnectedRealmIdPattern().Match(href).Groups[1].Value is string idStr && int.TryParse(idStr, out var id) ? id : 0;
}