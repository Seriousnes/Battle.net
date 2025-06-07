using Battle.net.SourceGenerator.Models;
using static Battle.net.SourceGenerator.Tests.Utility.Helpers;
using System.Reflection;

using Xunit.Abstractions;

namespace Battle.net.SourceGenerator.Tests;

public class CodeGenerationDebugTests(ITestOutputHelper output)
{
    [Fact]
    public void DebugGeneratedCode_ShouldShowActualOutput()
    {
        // Arrange
        var section = new ApiSection
        {
            Name = "Achievement",
            ClassName = "Achievement",
            Endpoints =
            [
                new ApiEndpoint
                {
                    Name = "Achievement",
                    MethodName = "GetAchievement",
                    HttpMethod = "GET",
                    Path = "/data/wow/achievement/{achievementId}",
                    Description = "Returns an index of achievements.",
                    Parameters = []
                },
                new ApiEndpoint
                {
                    Name = "Achievement",
                    MethodName = "GetAchievement",
                    HttpMethod = "GET",
                    Path = "/data/wow/achievement/{achievementId}",
                    Description = "Returns an achievement by ID.",
                    Parameters =
                    [
                        new ApiParameter
                        {
                            Name = "achievementId",
                            Type = "int",
                            IsRequired = true,
                            IsPathParameter = true,
                            Description = "The ID of the achievement."
                        }
                    ]
                },
                new ApiEndpoint
                {
                    Name = "Achievement Categories Index",
                    MethodName = "GetAchievementCategories",
                    HttpMethod = "GET",
                    Path = "/data/wow/achievement-category/index",
                    Description = "Returns an index of achievement categories.",
                    Parameters = []
                },
                new ApiEndpoint
                {
                    Name = "Achievement Category",
                    MethodName = "GetAchievementCategory",
                    HttpMethod = "GET",
                    Path = "/data/wow/achievement-category/{achievementCategoryId}",
                    Description = "Returns an achievement category by ID.",
                    Parameters =
                    [
                        new ApiParameter
                        {
                            Name = "achievementCategoryId",
                            Type = "int",
                            IsRequired = true,
                            IsPathParameter = true,
                            Description = "The ID of the achievement category."
                        }
                    ]
                },
                new ApiEndpoint
                {
                    Name = "Achievement Media",
                    MethodName = "GetAchievementMedia",
                    HttpMethod = "GET",
                    Path = "/data/wow/media/achievement/{achievementId}",
                    Description = "Returns media for an achievement by ID.",
                    Parameters =
                    [
                        new ApiParameter
                        {
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

        var generatedCode = (string?)generateApiClassMethod?.Invoke(generator, [section, classTemplate, methodTemplate]) ?? "";

        // Assert - Output the actual generated code for debugging
        output.WriteLine("=== GENERATED CODE ===");
        output.WriteLine(generatedCode);
        output.WriteLine("=== END GENERATED CODE ===");

        Assert.NotEmpty(generatedCode);
    }
}
