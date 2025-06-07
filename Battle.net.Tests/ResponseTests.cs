using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Battle.net.WorldOfWarcraft;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;
using Battle.net.WorldOfWarcraft.Models;

namespace Battle.net.Tests;

public class ResponseTests
{
    private static readonly JsonSerializerOptions jso = new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = true,
        IncludeFields = true,
    };

    private readonly ITestOutputHelper _outputHelper;
    private string responsePath;

    public ResponseTests(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;

        ApiRequestExecutor.Initialize(() =>
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            responsePath = config.GetValue<string>("ResponseModels")!;

            return config.GetSection("OAuthToken");
        });
    }

    [Fact]
    public void ExecuteAllIndexMethodsAndSaveAsJson()
    {
        // Create the responsemodels directory if it doesn't exist
        if (!Directory.Exists(responsePath))
        {
            Directory.CreateDirectory(responsePath);
        }

        // Get the GameData class to access all the API functions
        var api = new ApiRequestExecutor();
        var client = new ApiClient(api);
        var gameData = client.GameData;
        
        // Get all properties of GameData, each representing a different API area
        var gameDataProperties = typeof(GameData).GetProperties();
        
        foreach (var property in gameDataProperties)
        {
            // Get the value (instance) of the property
            var propertyValue = property.GetValue(gameData);
            if (propertyValue == null) continue;

            // Get the type of the property value
            var propertyType = propertyValue.GetType();
            
            // Find all methods in the property type that contain "Index" in their name
            var indexMethods = propertyType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(m => m.Name.Contains("Index", StringComparison.OrdinalIgnoreCase) && 
                           m.GetParameters().Length == 0) // Only parameterless methods
                .ToList();
            
            foreach (var indexMethod in indexMethods)
            {
                // Skip methods where we already a response file
                var filename = $"{property.Name}_{indexMethod.Name}.json";
                var filePath = Path.Combine(responsePath, filename);
                if (File.Exists(filePath))
                {
                    _outputHelper.WriteLine($"Skipping {property.Name}.{indexMethod.Name} as response file already exists: {filePath}");
                    continue;
                }

                try
                {
                    _outputHelper.WriteLine($"Executing {property.Name}.{indexMethod.Name}");

                    // Rate limit requests to avoid hitting API limits                    
                    Thread.Sleep(100); // Sleep for 0.1 second between requests

                    // Execute the method
                    var result = indexMethod.Invoke(propertyValue, null) ?? throw new InvalidOperationException($"Method {indexMethod.Name} returned null.");
                    
                    // Serialize the result to JSON and save to file
                    var json = JsonSerializer.Serialize(result, result.GetType(), jso);
                    File.WriteAllText(filePath, json);
                    
                    _outputHelper.WriteLine($"Saved result to {filePath}");
                }
                catch (Exception ex)
                {
                    _outputHelper.WriteLine($"Error executing {property.Name}.{indexMethod.Name}: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        _outputHelper.WriteLine($"Inner exception: {ex.InnerException.Message}");
                    }
                    break; // Stop on first error to avoid flooding output with errors
                }
            }
        }
    }
}
