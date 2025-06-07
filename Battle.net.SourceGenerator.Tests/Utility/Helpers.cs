namespace Battle.net.SourceGenerator.Tests.Utility;

public static class Helpers
{
    public static string GetEmbeddedTemplate(string templateName)
    {
        var assembly = typeof(BattleNetApiSourceGenerator).Assembly;
        using var stream = assembly.GetManifestResourceStream($"Battle.net.SourceGenerator.Templates.{templateName}") ?? throw new InvalidOperationException($"Template '{templateName}' not found in embedded resources.");
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
