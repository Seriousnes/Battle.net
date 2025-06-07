using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

using Battle.net.SourceGenerator.Models;
using Battle.net.SourceGenerator.Parsers;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Battle.net.SourceGenerator;

[Generator]
public class BattleNetApiSourceGenerator : IIncrementalGenerator
{
    private static readonly Regex PathParameterRegex = new(@"\{(\w+)\}", RegexOptions.Compiled);


    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Get all additional text files
        var additionalFiles = context.AdditionalTextsProvider;

        // Filter and collect HTML files
        var htmlFiles = additionalFiles
            .Where(file => file.Path.EndsWith(".html", StringComparison.OrdinalIgnoreCase))
            .Collect();            // Register source output
        context.RegisterSourceOutput(htmlFiles, (sourceContext, htmlFileArray) =>
        {
            foreach (var htmlFile in htmlFileArray)
            {
                var content = htmlFile.GetText()?.ToString();
                if (!string.IsNullOrEmpty(content))
                {
                    Execute(sourceContext, content!);
                    break; // We only need one HTML file
                }
            }
        });
    }


    private void Execute(SourceProductionContext context, string htmlContent)
    {
        try
        {
            // Debug: Report that Execute is being called
            context.ReportDiagnostic(Diagnostic.Create(
                new DiagnosticDescriptor("BNSG999", "Debug",
                    $"Execute called with content length: {htmlContent?.Length ?? 0}",
                    "BattleNetSourceGenerator", DiagnosticSeverity.Info, true),
                Location.None));

            if (string.IsNullOrEmpty(htmlContent))
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    new DiagnosticDescriptor("BNSG001", "HTML Documentation Not Found",
                        "Could not find HTML documentation file in additional files",
                        "BattleNetSourceGenerator", DiagnosticSeverity.Warning, true),
                    Location.None));
                return;
            }

            // Parse the HTML documentation
            var parser = new HtmlApiDocumentationParser();
            var sections = parser.ParseDocumentation(htmlContent!);

            context.ReportDiagnostic(Diagnostic.Create(
                new DiagnosticDescriptor("BNSG998", "Debug",
                    $"Parsed {sections.Count} sections from HTML",
                    "BattleNetSourceGenerator", DiagnosticSeverity.Info, true),
                Location.None));

            // Get templates from embedded resources
            var classTemplate = GetEmbeddedResource("ApiClassTemplate.txt");
            var methodTemplate = GetEmbeddedResource("ApiMethodTemplate.txt");

            if (string.IsNullOrEmpty(classTemplate) || string.IsNullOrEmpty(methodTemplate))
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    new DiagnosticDescriptor("BNSG002", "Templates Not Found",
                        $"Could not find code generation templates in embedded resources. ClassTemplate: {classTemplate?.Length ?? 0} chars, MethodTemplate: {methodTemplate?.Length ?? 0} chars",
                        "BattleNetSourceGenerator", DiagnosticSeverity.Error, true),
                    Location.None));
                return;
            }

            foreach (var section in sections)
            {
                if (section.Endpoints.Count == 0) continue;

                var generatedCode = GenerateApiClass(section, classTemplate, methodTemplate);
                var fileName = $"{section.ClassName}.g.cs";

                context.ReportDiagnostic(Diagnostic.Create(
                    new DiagnosticDescriptor("BNSG997", "Debug",
                        $"Generated file: {fileName} with {generatedCode.Length} characters",
                        "BattleNetSourceGenerator", DiagnosticSeverity.Info, true),
                    Location.None));

                context.AddSource(fileName, SourceText.From(generatedCode, Encoding.UTF8));
            }
        }
        catch (Exception ex)
        {
            context.ReportDiagnostic(Diagnostic.Create(
                new DiagnosticDescriptor("BNSG003", "Source Generation Error",
                    $"An error occurred during source generation: {ex.Message}",
                    "BattleNetSourceGenerator", DiagnosticSeverity.Error, true),
                Location.None));
        }
    }

    private string GenerateApiClass(ApiSection section, string classTemplate, string methodTemplate)
    {
        var methods = new StringBuilder();
        var interfaceMethods = new StringBuilder();

        foreach (var endpoint in section.Endpoints)
        {
            var method = GenerateMethod(endpoint, methodTemplate);
            methods.AppendLine(method);
            methods.AppendLine();

            // Generate interface method
            var interfaceMethod = GenerateInterfaceMethod(endpoint);
            interfaceMethods.AppendLine(interfaceMethod);
        }

        return classTemplate
            .Replace("{{CLASS_NAME}}", section.ClassName)
            .Replace("{{SECTION_DESCRIPTION}}", $"API methods for {section.Name}")
            .Replace("{{METHODS}}", methods.ToString().TrimEnd())
            .Replace("{{INTERFACE_METHODS}}", interfaceMethods.ToString().TrimEnd());
    }

    private string GenerateMethod(ApiEndpoint endpoint, string methodTemplate)
    {
        // Generate method parameters - include path parameters and query parameters
        var parameters = new StringBuilder();

        // Add path parameters first
        foreach (var param in endpoint.Parameters.Where(p => p.IsPathParameter))
        {
            if (parameters.Length > 0) parameters.Append(", ");
            parameters.Append($"{param.Type} {ConvertToCamelCase(param.Name)}");
        }

        // Add query parameters as optional parameters
        foreach (var param in endpoint.QueryParameters)
        {
            if (parameters.Length > 0) parameters.Append(", ");
            var paramType = param.IsRequired ? param.Type : $"{param.Type}?";
            var defaultValue = param.IsRequired ? "" : " = null";
            parameters.Append($"{paramType} {ConvertToCamelCase(param.Name)}{defaultValue}");
        }

        // Generate base parameters (region, namespace, locale) with defaults
        var baseParameters = new StringBuilder();
        var hasRegion = endpoint.Parameters.Any(p => p.BaseParameterName == "region");
        var hasNamespace = endpoint.Parameters.Any(p => p.BaseParameterName == "@namespace");
        var hasLocale = endpoint.Parameters.Any(p => p.BaseParameterName == "locale");

        if (hasRegion || hasNamespace || hasLocale)
        {
            baseParameters.Append(", ");

            if (hasRegion)
            {
                baseParameters.Append("region");
            }
            else
            {
                baseParameters.Append("Region.US");
            }

            baseParameters.Append(", ");

            if (hasNamespace)
            {
                baseParameters.Append("@namespace");
            }
            else
            {
                baseParameters.Append("Namespace.Static");
            }

            if (hasLocale)
            {
                baseParameters.Append(", locale");
            }
        }

        // Replace path parameters with actual parameter values
        var apiPath = endpoint.Path;
        var pathParams = PathParameterRegex.Matches(apiPath);
        foreach (Match match in pathParams)
        {
            var paramName = match.Groups[1].Value;
            var camelCaseName = ConvertToCamelCase(paramName);
            apiPath = apiPath.Replace($"{{{paramName}}}", $"{{{camelCaseName}}}");
        }            
        
        // Add query parameters to the URL if any exist
        if (endpoint.QueryParameters.Count > 0)
        {
            var queryParts = new List<string>();
            foreach (var param in endpoint.QueryParameters)
            {
                var paramVarName = ConvertToCamelCase(param.Name);
                if (param.IsRequired)
                {
                    queryParts.Add($"{param.Name}={{{paramVarName}}}");
                }
                else
                {
                    queryParts.Add($"{{({paramVarName} != null ? $\"{param.Name}={{{paramVarName}}}\" : \"\")}}");
                }
            }
            apiPath += "?" + string.Join("&", queryParts);
        }

        // Generate return type based on endpoint name
        var returnType = GenerateReturnType(endpoint.Name);

        return methodTemplate
            .Replace("{{METHOD_NAME}}", endpoint.MethodName)
            .Replace("{{PARAMETERS}}", parameters.ToString())
            .Replace("{{API_PATH}}", apiPath)
            .Replace("{{BASE_PARAMETERS}}", baseParameters.ToString())
            .Replace("{{RETURN_TYPE}}", returnType);
    }

    private string GenerateInterfaceMethod(ApiEndpoint endpoint)
    {
        // Generate method parameters - include path parameters and query parameters
        var parameters = new StringBuilder();

        // Add path parameters first
        foreach (var param in endpoint.Parameters.Where(p => p.IsPathParameter))
        {
            if (parameters.Length > 0) parameters.Append(", ");
            parameters.Append($"{param.Type} {ConvertToCamelCase(param.Name)}");
        }

        // Add query parameters as optional parameters
        foreach (var param in endpoint.QueryParameters)
        {
            if (parameters.Length > 0) parameters.Append(", ");
            var paramType = param.IsRequired ? param.Type : $"{param.Type}?";
            var defaultValue = param.IsRequired ? "" : " = null";
            parameters.Append($"{paramType} {ConvertToCamelCase(param.Name)}{defaultValue}");
        }

        // Generate return type based on endpoint name
        var returnType = GenerateReturnType(endpoint.Name);

        return $"    {returnType} {endpoint.MethodName}({parameters});";
    }

    private string GenerateReturnType(string endpointName)
    {
        // Map endpoint names to model types based on Achievement.cs pattern
        return endpointName.ToLowerInvariant() switch
        {
            "achievement" => "AchievementModel",
            "achievements index" => "AchievementIndexModel",
            "achievement categories index" => "AchievementCategoriesModel",
            "achievement category" => "AchievementCategoryModel",
            "achievement media" => "AchievementMediaModel",
            _ => "object" // Default fallback
        };
    }

    private string ConvertToCamelCase(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;

        // Handle the case where input is already in camelCase or PascalCase
        if (input.Contains("Id"))
        {
            // If it already contains "Id", just convert the first character to lowercase
            return char.ToLowerInvariant(input[0]) + input.Substring(1);
        }

        var words = input.Split(['_', '-', ' '], StringSplitOptions.RemoveEmptyEntries);
        if (words.Length == 0) return input;

        var result = words[0].ToLowerInvariant();
        for (int i = 1; i < words.Length; i++)
        {
            // Handle special case for "Id" to keep proper casing
            if (words[i].Equals("id", StringComparison.OrdinalIgnoreCase))
            {
                result += "Id";
            }
            else
            {
                result += char.ToUpperInvariant(words[i][0]) + words[i].Substring(1).ToLowerInvariant();
            }
        }

        return result;
    }

    private string GetEmbeddedResource(string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var fullResourceName = $"Battle.net.SourceGenerator.Templates.{resourceName}";

        using var stream = assembly.GetManifestResourceStream(fullResourceName);
        if (stream == null) return string.Empty;

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
