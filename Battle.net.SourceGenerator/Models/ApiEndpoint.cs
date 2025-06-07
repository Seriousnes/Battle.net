using System.Collections.Generic;
using System.Diagnostics;

namespace Battle.net.SourceGenerator.Models;

[DebuggerDisplay("{Path}")]
public class ApiEndpoint
{
    public string Name { get; set; } = string.Empty;
    public string HttpMethod { get; set; } = "GET";
    public string Path { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<ApiParameter> Parameters { get; set; } = [];
    public List<ApiParameter> QueryParameters { get; set; } = [];
    public string ReturnType { get; set; } = "object";
    public string MethodName { get; set; } = string.Empty;
}
