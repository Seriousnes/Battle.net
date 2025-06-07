using System.Text.Json.Serialization;

namespace Battle.net.WorldOfWarcraft.Models.AuctionHouse;

public class Item
{
    public int Id { get; set; }
    public int Context { get; set; }
    public List<Modifier> Modifiers { get; set; }
    [JsonPropertyName("bonus_lists")]
    public List<int> BonusLists { get; set; }
    [JsonPropertyName("pet_breed_id")]
    public int PetBreedId { get; set; }
    [JsonPropertyName("pet_level")]
    public int PetLevel { get; set; }
    [JsonPropertyName("pet_quality_id")]
    public int PetQualityId { get; set; }
    [JsonPropertyName("pet_species_id")]
    public int PetSpeciesId { get; set; }
}