namespace Battle.net.SourceGenerator.Models;

public class ApiParameter
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = "string";
    public bool IsRequired { get; set; } = true;
    public string Description { get; set; } = string.Empty;
    public string? DefaultValue { get; set; }
    public bool IsPathParameter { get; set; } = false;
    public bool IsBaseParameter { get; set; } = false; // Maps to BaseFunction parameters (region, namespace, locale)
    public string? BaseParameterName { get; set; } // The actual parameter name in BaseFunction
}
