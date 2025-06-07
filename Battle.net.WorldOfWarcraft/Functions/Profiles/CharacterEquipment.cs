using Battle.net.WorldOfWarcraft.Models.Profiles;

namespace Battle.net.WorldOfWarcraft.Functions.Profiles;

public class CharacterEquipment(IApiRequestExecutor api) : BaseProfileFunction(api), ICharacterEquipment
{
    public CharacterEquipmentModel GetCharacterEquipmentSummary(string realm, string characterName)
    {
        return ApiRequest<CharacterEquipmentModel>($"/profile/wow/character/{realm}/{characterName}/equipment", locale: Locale.en_US);
    }
}

public interface ICharacterEquipment
{
    CharacterEquipmentModel GetCharacterEquipmentSummary(string realm, string characterName);
}