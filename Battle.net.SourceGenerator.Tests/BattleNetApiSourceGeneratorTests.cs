using Battle.net.SourceGenerator.Models;
using static Battle.net.SourceGenerator.Tests.Utility.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;

namespace Battle.net.SourceGenerator.Tests;

public class BattleNetApiSourceGeneratorTests
{
    [Fact]
    public void Generator_ShouldGenerateValidCSharpCode()
    {
        // Arrange
        var generator = new BattleNetApiSourceGenerator();
        var compilation = CreateCompilation("");
        var driver = CSharpGeneratorDriver.Create(generator);

        // Act
        var runResult = driver.RunGenerators(compilation);

        // Assert
        var result = runResult.GetRunResult();
        Assert.True(result.Diagnostics.IsEmpty,
            $"Generator produced diagnostics: {string.Join(", ", result.Diagnostics.Select(d => d.ToString()))}");
    }

    [Fact]
    public void GeneratedCode_ShouldMatchExpectedAchievementApiPattern()
    {
        // Arrange
        var section = new ApiSection
        {
            Name = "Achievement",
            ClassName = "WoWAchievementApi",
            Endpoints =
            [
                new() {
                    Name = "Achievements Index",
                    MethodName = "GetAchievementsIndex",
                    HttpMethod = "GET",
                    Path = "/data/wow/achievement/index",
                    Description = "Returns an index of achievements.",
                    Parameters = []
                },
                new() {
                    Name = "Achievement",
                    MethodName = "GetAchievement",
                    HttpMethod = "GET",
                    Path = "/data/wow/achievement/{achievementId}",
                    Description = "Returns an achievement by ID.",
                    Parameters =
                    [
                        new() {
                            Name = "achievementId",
                            Type = "int",
                            IsRequired = true,
                            IsPathParameter = true,
                            Description = "The ID of the achievement."
                        }
                    ]
                },
                new() {
                    Name = "Achievement Categories Index",
                    MethodName = "GetAchievementCategoriesIndex",
                    HttpMethod = "GET",
                    Path = "/data/wow/achievement-category/index",
                    Description = "Returns an index of achievement categories.",
                    Parameters = []
                },
                new() {
                    Name = "Achievement Category",
                    MethodName = "GetAchievementCategory",
                    HttpMethod = "GET",
                    Path = "/data/wow/achievement-category/{achievementCategoryId}",
                    Description = "Returns an achievement category by ID.",
                    Parameters =
                    [
                        new() {
                            Name = "achievementCategoryId",
                            Type = "int",
                            IsRequired = true,
                            IsPathParameter = true,
                            Description = "The ID of the achievement category."
                        }
                    ]
                },
                new() {
                    Name = "Achievement Media",
                    MethodName = "GetAchievementMedia",
                    HttpMethod = "GET",
                    Path = "/data/wow/media/achievement/{achievementId}",
                    Description = "Returns media for an achievement by ID.",
                    Parameters =
                    [
                        new() {
                            Name = "achievementId",
                            Type = "int",
                            IsRequired = true,
                            IsPathParameter = true,
                            Description = "The ID of the achievement."
                        }
                    ]
                }
            ]
        };

        var classTemplate = GetEmbeddedTemplate("ApiClassTemplate.txt");
        var methodTemplate = GetEmbeddedTemplate("ApiMethodTemplate.txt");        
        
        // Act
        var generator = new BattleNetApiSourceGenerator();
        var generateApiClassMethod = typeof(BattleNetApiSourceGenerator).GetMethod("GenerateApiClass",
            BindingFlags.NonPublic | BindingFlags.Instance);

        var generatedCode = (string?)generateApiClassMethod?.Invoke(generator,
            [section, classTemplate, methodTemplate]) ?? "";        
        
        // Assert
        // Debug: output the actual generated code
        Console.WriteLine("=== ACTUAL GENERATED CODE ===");
        Console.WriteLine(generatedCode);
        Console.WriteLine("=== END GENERATED CODE ===");

        Assert.Contains("GetAchievementsIndex", generatedCode);
        Assert.Contains("GetAchievement", generatedCode);
        Assert.Contains("GetAchievementCategoriesIndex", generatedCode);
        Assert.Contains("GetAchievementCategory", generatedCode);
        Assert.Contains("GetAchievementMedia", generatedCode);

        // Verify parameter handling
        Assert.Contains("int achievementId", generatedCode);
        Assert.Contains("int achievementCategoryId", generatedCode);

        // Verify path parameter replacement
        Assert.Contains("/data/wow/achievement/{achievementId}", generatedCode);
        Assert.Contains("/data/wow/achievement-category/{achievementCategoryId}", generatedCode);
        Assert.Contains("/data/wow/media/achievement/{achievementId}", generatedCode);

        // Verify it compiles
        var compilation = CreateCompilation(generatedCode);
        var diagnostics = compilation.GetDiagnostics()
            .Where(d => d.Severity == DiagnosticSeverity.Error)
            .ToList();

        Assert.Empty(diagnostics);
    }

    [Fact]
    public void GeneratedCode_ShouldMatchExistingAchievementFunctionSignature()
    {
        // This test verifies that our generated methods would be compatible 
        // with the existing Achievement.cs pattern

        // Arrange
        var section = new ApiSection
        {
            Name = "Achievement",
            ClassName = "WoWAchievementApi",
            Endpoints =
            [
                new() {
                    Name = "Achievement",
                    MethodName = "GetAchievement",
                    HttpMethod = "GET",
                    Path = "/data/wow/achievement/{achievementId}",
                    Description = "Returns an achievement by ID.",
                    Parameters =
                    [
                        new() {
                            Name = "achievementId",
                            Type = "int",
                            IsRequired = true,
                            IsPathParameter = true,
                            Description = "The ID of the achievement."
                        }
                    ]
                }
            ]
        };

        var classTemplate = GetEmbeddedTemplate("ApiClassTemplate.txt");
        var methodTemplate = GetEmbeddedTemplate("ApiMethodTemplate.txt");        
        
        // Act
        var generator = new BattleNetApiSourceGenerator();
        var generateApiClassMethod = typeof(BattleNetApiSourceGenerator).GetMethod("GenerateApiClass",
            BindingFlags.NonPublic | BindingFlags.Instance);

        var generatedCode = (string?)generateApiClassMethod?.Invoke(generator,
            [section, classTemplate, methodTemplate]) ?? "";

        // Assert - Compare with existing Achievement.cs pattern
        Assert.Contains("public AchievementModel GetAchievement(int achievementId)", generatedCode);
        Assert.Contains("return ApiRequest<AchievementModel>($\"/achievement/${achievementId}\");", generatedCode);        
        Assert.Contains("achievement/{achievementId}", generatedCode);
    }

    private static Compilation CreateCompilation(string source)
    {
        var references = new[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Task).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(System.Collections.Generic.Dictionary<,>).Assembly.Location)
        };

        return CSharpCompilation.Create(
            "TestAssembly",
            [CSharpSyntaxTree.ParseText(source)],
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
    }
}
