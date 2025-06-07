---
applyTo: '**'
---

# Battle.net World of Warcraft API Client Instructions

This is a C# .NET 8.0 library that provides a comprehensive client for accessing the Battle.net World of Warcraft API. The library is structured to provide easy access to both Game Data and Profile endpoints.

## Project Structure

### Core Components
- **ApiClient.cs**: Main entry point that provides access to GameData and Profiles
- **Config.cs**: Configuration class containing OAuth credentials
- **GameData.cs**: Aggregates all game data API functions
- **Profiles.cs**: Aggregates all profile-related API functions

### Key Directories
- **Functions/GameData/**: Contains all game data API endpoint implementations
- **Functions/Profiles/**: Contains all profile-related API endpoint implementations  
- **Models/**: Contains all data models and DTOs organized by API category
- **Enums/**: Contains enumerations for Region, Namespace, and Locale

## Architecture Patterns

### Dependency Injection
The library uses constructor-based dependency injection:
```csharp
public class ApiClient(IApiRequestExecutor api)
public class GameData(IApiRequestExecutor api) : IGameData
public class AchievementFunction(IApiRequestExecutor api) : BaseFunction(api), IAchievementFunction
```

### Interface Segregation
Each API category has its own interface:
- `IGameData` - Aggregates all game data functions
- `IAchievementFunction`, `IItemFunction`, etc. - Individual API category interfaces
- `IApiRequestExecutor` - HTTP request execution abstraction

### Base Classes
- **BaseFunction**: Provides common API request functionality for all GameData functions
- **BaseProfileFunction**: Provides common functionality for Profile functions
- **MediaDocument**: Base class for models that include media/asset information

## Authentication & Configuration

### OAuth 2.0 Client Credentials Flow
The library handles OAuth token management automatically:
- Tokens are refreshed when expired or expiring
- Uses client credentials grant type
- Stores credentials in the `Config` class

### Regional Support
- Default region: US
- Supports all Battle.net regions via `Region` enum
- Region can be specified per request or use defaults

### Localization
- Default locale: en_US
- Supports all WoW locales via `Locale` enum
- Locale can be specified per request

## API Organization

### Game Data APIs
Comprehensive coverage of WoW game data including:
- Achievements, Items, Characters, Guilds
- Mythic+ dungeons and leaderboards
- PvP seasons and tiers
- Professions, talents, spells
- Auction house data
- And many more...

### Profile APIs  
Character and account-specific data:
- Character profiles, equipment, achievements
- Character collections, pets, mounts
- Guild information
- Account profiles
- Mythic+ and PvP profiles

## Development Guidelines

### Adding New Endpoints
1. Create the model classes in appropriate `Models/` subdirectory
2. Create the function class inheriting from `BaseFunction` or `BaseProfileFunction`
3. Define the interface for the function
4. Add the function to the appropriate aggregator (`GameData` or `Profiles`)
5. Follow the existing naming conventions and patterns

### Model Conventions
- Use `System.Text.Json` attributes for JSON serialization
- Inherit from `MediaDocument` if the model includes media assets
- Inherit from `KeyModel` for models with key-value references
- Use proper nullable annotations

### Function Conventions
- Implement both sync and async versions when possible
- Use descriptive parameter names matching the API documentation
- Include proper error handling
- Follow the established URL pattern conventions

### Error Handling
- HTTP errors are handled at the `ApiRequestExecutor` level
- Failed JSON deserialization writes debug files with timestamp
- Null responses throw `HttpRequestException`

## Dependencies
- **RestSharp 112.0.0**: Primary HTTP client library
- **.NET 8.0**: Target framework with latest language features
- **System.Text.Json**: JSON serialization (built-in)

## Best Practices
- Always use interfaces for dependency injection
- Maintain consistent naming conventions across functions and models
- Include proper XML documentation for public APIs
- Use nullable reference types appropriately
- Follow async/await patterns for better performance
- Implement proper disposal patterns for HTTP resources

## Testing Considerations
- Mock `IApiRequestExecutor` for unit testing
- Test both successful responses and error conditions
- Verify proper OAuth token management
- Test regional and locale parameter handling