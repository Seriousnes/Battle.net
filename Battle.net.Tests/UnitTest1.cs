using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

using Battle.net.WorldOfWarcraft;
using Battle.net.WorldOfWarcraft.Models;
using Battle.net.WorldOfWarcraft.Models.;
using Battle.net.WorldOfWarcraft.Models.AuctionHouse;
using Battle.net.WorldOfWarcraft.Models.Profiles;

using FluentAssertions;

using Microsoft.Extensions.Configuration;

using Xunit.Abstractions;

namespace Battle.net.Tests;

public class UnitTest1
{
    private static readonly JsonSerializerOptions jso = new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        IncludeFields = true,
    };
    private readonly ITestOutputHelper outputHelper;

    public UnitTest1(ITestOutputHelper outputHelper)
    {
        this.outputHelper = outputHelper;

        ApiRequestExecutor.Initialize(() =>
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            return config.GetSection("OAuthToken");
        });
    }

    [Fact(Skip = "API Tests")]
    public void GetCharacterEquipmentFromAPI()
    {
        var api = new ApiRequestExecutor();
        var client = new ApiClient(api);

        var equipment = client.Profile.CharacterEquipment.GetCharacterEquipmentSummary("frostmourne", "seriousnes");
        equipment.Should().NotBeNull();
    }

    [Fact]
    public void GetCharacterEquipmentFromJson()
    {
        var ce = File.ReadAllText("characterEquipment.json");
        var ceModel = JsonSerializer.Deserialize<CharacterEquipmentModel>(ce, jso);
        ceModel.Character.Should().NotBeNull();
        ceModel.Character.Name.Should().Be("Seriousnes");
    }

    [Fact(Skip = "API Tests")]
    public void GetAuctionsFromAPI()
    {
        var api = new ApiRequestExecutor();
        var client = new ApiClient(api);

        var auctionsModel = client.GameData.AuctionHouse.GetAuctions(3725); // Frostmourne
        //auctionsModel.Auctions.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public void GetAuctionsFromJson()
    {
        var auctions = File.ReadAllText("auctions.json");
        var auctionsModel = JsonSerializer.Deserialize<AuctionsModel>(auctions, jso);
    }

    [Fact]
    public void GetConnectedRealmFromAPI()
    {
        var api = new ApiRequestExecutor();
        var client = new ApiClient(api);
        var connectedRealm = client.GameData.ConnectedRealms.GetConnectedRealm(3725); // Frostmourne
        connectedRealm.Should().NotBeNull();
        //connectedRealm.Realms.Count().Should().BeGreaterThan(0);
    }

    [Fact]
    public void GetConnectedRealmFromJson()
    {
        var cr = File.ReadAllText("connectedrealm.json");
        var connectedRealm = JsonSerializer.Deserialize<ConnectedRealmModel>(cr, jso);
        connectedRealm.Should().NotBeNull();
        connectedRealm.Realms.Count().Should().BeGreaterThan(0);
    }

    [Fact]
    public void GetConnectedRealmsFromAPI()
    {
        var api = new ApiRequestExecutor();
        var client = new ApiClient(api);
        var connectedRealms = client.GameData.ConnectedRealms.GetConnectedRealmsIndex(); // Frostmourne
        connectedRealms.Should().NotBeNull();
        //connectedRealms.ConnectedRealms.Count().Should().BeGreaterThan(0);
    }

    [Fact]
    public void GetConnectedRealmsFromJson()
    {
        var cr = File.ReadAllText("connectedrealms.json");
        var crModel = JsonSerializer.Deserialize<ConnectedRealmsModel>(cr, jso);
        crModel.Should().NotBeNull();
        crModel.ConnectedRealms.Count().Should().BeGreaterThan(0);
    }
}