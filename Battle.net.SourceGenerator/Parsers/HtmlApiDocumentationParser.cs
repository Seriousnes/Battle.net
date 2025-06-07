using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

using Battle.net.SourceGenerator.Models;

using HtmlAgilityPack;

namespace Battle.net.SourceGenerator.Parsers;

public class HtmlApiDocumentationParser
{
    private static readonly Regex PathParameterRegex = new(@"\{(\w+)\}", RegexOptions.Compiled);    
    private static readonly TextInfo TextInfo = new CultureInfo("en-US", false).TextInfo;

    public List<ApiSection> ParseDocumentation(string htmlContent)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(htmlContent);

        var sections = new List<ApiSection>();

        // Find all API sections by looking for toolbar elements with API names
        var toolbarElements = doc.DocumentNode
            .Descendants(0)
            .Where(x => x.HasClass("mat-toolbar"))
            //?.Where(n => n.InnerText.Trim().EndsWith(" API"))
            ?.ToList();

        if (toolbarElements == null) return sections;

        foreach (var toolbar in toolbarElements)
        {
            var innerText = toolbar.FirstChild.InnerText.Trim();
            var apiIndex = innerText.IndexOf(" API");
            var sectionName = apiIndex >= 0 ? innerText.Substring(0, apiIndex) : innerText;
            if (string.IsNullOrEmpty(sectionName) || sections.Any(x => x.Name == sectionName))
            {
                continue;
            }

            var section = new ApiSection
            {
                Name = sectionName,
                ClassName = GenerateClassName(sectionName)
            };

            // Find the parent container that holds the methods
            var methodsContainer = toolbar.Ancestors()
                .FirstOrDefault(n => n.Name == "app-api-reference-resource")
                ?.SelectSingleNode(".//div[@class='methods']");

            if (methodsContainer != null)
            {
                section.Endpoints = ParseEndpoints(methodsContainer);
            }

            sections.Add(section);
        }

        return sections;
    }

    private List<ApiEndpoint> ParseEndpoints(HtmlNode methodsContainer)
    {
        var endpoints = new List<ApiEndpoint>();

        var expansionPanels = methodsContainer
            .SelectNodes(".//mat-expansion-panel[contains(@class, 'method-panel-header')]")
            ?.ToList();

        if (expansionPanels == null) return endpoints;

        foreach (var panel in expansionPanels)
        {
            var endpoint = ParseEndpoint(panel);
            if (endpoint != null)
            {
                // Check if an endpoint with the same method name already exists
                var existingEndpoint = endpoints.FirstOrDefault(e => e.MethodName == endpoint.MethodName);
                if (existingEndpoint == null)
                {
                    endpoints.Add(endpoint);
                }
                // If duplicate exists, we skip adding it (keeping the first one found)
            }
        }

        return endpoints;
    }

    private ApiEndpoint? ParseEndpoint(HtmlNode panel)
    {
        try
        {
            // Extract method name and HTTP method
            var methodNameNode = panel.SelectSingleNode(".//div[@class='method-name']");
            var httpMethodNode = panel.SelectSingleNode(".//code[@class='method-type']");
            var pathNode = panel.SelectSingleNode(".//div[@class='blz-text-ellipsis']");

            if (methodNameNode == null || httpMethodNode == null || pathNode == null)
                return null;

            var name = CleanText(methodNameNode.InnerText);
            var httpMethod = CleanText(httpMethodNode.InnerText);
            var path = CleanText(pathNode.InnerText);

            var endpoint = new ApiEndpoint
            {
                Name = name,
                HttpMethod = httpMethod,
                Path = path,
                MethodName = GenerateMethodName(name)
            };

            // Extract description
            var descriptionNode = panel.SelectSingleNode(".//section[@class='method-description ng-star-inserted']//p");
            if (descriptionNode != null)
            {
                endpoint.Description = CleanText(descriptionNode.InnerText);
            }

            // Extract parameters
            endpoint.Parameters = ParseParameters(panel);

            // Separate path parameters from query parameters
            var pathParams = PathParameterRegex.Matches(path);
            foreach (Match match in pathParams)
            {
                var paramName = match.Groups[1].Value;
                var param = endpoint.Parameters.FirstOrDefault(p =>
                    p.Name.Equals(paramName, StringComparison.OrdinalIgnoreCase) ||
                    p.Name.Equals($"{{{paramName}}}", StringComparison.OrdinalIgnoreCase));

                if (param != null)
                {
                    param.IsPathParameter = true;
                }
            }

            // Categorize base parameters (region, namespace, locale)
            foreach (var param in endpoint.Parameters)
            {
                if (!param.IsPathParameter)
                {
                    CategorizeParameter(param);
                }
            }

            endpoint.QueryParameters = [.. endpoint.Parameters.Where(p => !p.IsPathParameter && !p.IsBaseParameter)];

            return endpoint;
        }
        catch
        {
            return null;
        }
    }

    private List<ApiParameter> ParseParameters(HtmlNode panel)
    {
        var parameters = new List<ApiParameter>();

        var parameterRows = panel.SelectNodes(".//tr[contains(@class, 'ng-star-inserted')]")?.ToList();
        if (parameterRows == null) return parameters;

        foreach (var row in parameterRows)
        {
            var cells = row.SelectNodes("td")?.ToList();
            if (cells == null || cells.Count < 3) continue;

            var nameCell = cells[0];
            var typeCell = cells[1];
            var valueCell = cells[2];
            var descCell = cells.Count > 3 ? cells[3] : null;

            var paramName = CleanText(nameCell.InnerText).Trim('{', '}', ' ');
            if (string.IsNullOrEmpty(paramName)) continue;

            var paramType = ExtractParameterType(typeCell);
            var isRequired = typeCell.InnerText.Contains("Required");
            var defaultValue = valueCell.InnerText;
            var description = descCell != null ? CleanText(descCell.InnerText) : "";

            parameters.Add(new ApiParameter
            {
                Name = paramName,
                Type = paramType,
                IsRequired = isRequired,
                DefaultValue = defaultValue,
                Description = description,

            });
        }

        return parameters;
    }

    private string ExtractParameterType(HtmlNode typeCell)
    {
        var typeText = CleanText(typeCell.InnerText).ToLowerInvariant();

        if (typeText.Contains("integer") || typeText.Contains("int"))
            return "int";
        if (typeText.Contains("string"))
            return "string";
        if (typeText.Contains("boolean") || typeText.Contains("bool"))
            return "bool";
        if (typeText.Contains("array"))
            return "string[]";

        return "string"; // Default to string
    }

    private string GenerateClassName(string sectionName)
    {
        var className = Regex.Replace(sectionName, @"[^\w]", "");
        return $"{className}"; // e.g. "Achievement" -> "AchievementFunction"
    }

    private string GenerateMethodName(string endpointName)
    {        
        var cleanedName = Regex.Replace(endpointName, @"\s*\([^)]*\)", "").Trim();        
        var methodName = "Get" + string.Join("", TextInfo.ToTitleCase(cleanedName).Split([' ']));
        return methodName;
    }

    private string CleanText(string text)
    {
        return string.IsNullOrEmpty(text)
            ? string.Empty
            : text.Trim()
            .Replace("\\n", " ")
            .Replace("\\r", "")
            .Replace("\\t", " ")
            .Replace("  ", " ")
            .Trim();
    }

    private void CategorizeParameter(ApiParameter param)
    {
        // Map documentation parameter names to BaseFunction parameter names
        switch (param.Name.ToLowerInvariant())
        {
            case ":region":
            case "region":
                param.IsBaseParameter = true;
                param.BaseParameterName = "region";
                param.Type = "Region";
                param.DefaultValue = $"Region.{TextInfo.ToUpper(param.DefaultValue!)}";
                break;
            case "namespace":
                param.IsBaseParameter = true;
                param.BaseParameterName = "@namespace";
                param.Type = "Namespace";
                param.DefaultValue = $"Namaspace.{TextInfo.ToTitleCase(param.DefaultValue!)}";
                break;
            case "locale":
                param.IsBaseParameter = true;
                param.BaseParameterName = "locale";
                param.Type = "Locale?";
                param.DefaultValue = $"Locale.{param.DefaultValue!}";
                break;
        }
    }
}
