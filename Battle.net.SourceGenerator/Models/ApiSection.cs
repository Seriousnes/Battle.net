using System.Collections.Generic;

namespace Battle.net.SourceGenerator.Models;

public class ApiSection
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<ApiEndpoint> Endpoints { get; set; } = [];
    public string ClassName { get; set; } = string.Empty;
}
