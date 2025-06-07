using Battle.net.SourceGenerator.Parsers;

using Xunit.Abstractions;

namespace Battle.net.SourceGenerator.Tests;

public class HtmlApiDocumentationParserTests(ITestOutputHelper output)
{
    [Fact]
    public void ParseDocumentation_ShouldDeduplicateWoWTokenMethods()
    {
        // Arrange
        var parser = new HtmlApiDocumentationParser();
        var htmlContent = """
            <div>
                <app-api-reference-resource>
                    <blz-card>
                        <mat-card>
                            <blz-card-toolbar>
                                <mat-toolbar> WoW Token API </mat-toolbar>
                            </blz-card-toolbar>
                            <div class="methods">
                                <mat-expansion-panel class="method-panel-header">
                                    <div class="method-name"> WoW Token Index (US, EU, KR, TW) </div>
                                    <code class="method-type">GET</code>
                                    <div class="blz-text-ellipsis">/data/wow/token/index</div>
                                    <section class="method-description ng-star-inserted">
                                        <p>Returns the WoW Token index.</p>
                                    </section>
                                </mat-expansion-panel>
                                <mat-expansion-panel class="method-panel-header">
                                    <div class="method-name"> WoW Token Index (CN) </div>
                                    <code class="method-type">GET</code>
                                    <div class="blz-text-ellipsis">/data/wow/token/index</div>
                                    <section class="method-description ng-star-inserted">
                                        <p>Returns the WoW Token index.</p>
                                    </section>
                                </mat-expansion-panel>
                            </div>
                        </mat-card>
                    </blz-card>
                </app-api-reference-resource>
            </div>
            """;

        // Act
        var sections = parser.ParseDocumentation(htmlContent);

        // Assert
        Assert.Single(sections);
        var wowTokenSection = sections[0];
        Assert.Equal("WoW Token", wowTokenSection.Name);
        
        // Should only have one endpoint after deduplication
        Assert.Single(wowTokenSection.Endpoints);
        var endpoint = wowTokenSection.Endpoints[0];
        Assert.Equal("GetWowTokenIndex", endpoint.MethodName);
        Assert.Equal("WoW Token Index (US, EU, KR, TW)", endpoint.Name); // Should keep the first one found
        Assert.Equal("/data/wow/token/index", endpoint.Path);

        output.WriteLine($"Generated method: {endpoint.MethodName}");
        output.WriteLine($"Original name: {endpoint.Name}");
        output.WriteLine($"Path: {endpoint.Path}");
    }

    [Fact]
    public void ParseDocumentation_ShouldIncludeRequiredParameters()
    {
        var htmlContent = File.ReadAllText("..\\..\\..\\..\\Battle.net.WorldOfWarcraft\\Documentation\\Game Data APIs _ Documentation.html");

        var parser = new HtmlApiDocumentationParser();

        var sections = parser.ParseDocumentation(htmlContent);
    }
}
