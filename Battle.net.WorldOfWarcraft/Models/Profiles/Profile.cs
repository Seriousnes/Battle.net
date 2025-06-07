using Battle.net.WorldOfWarcraft.Functions.Profiles;

namespace Battle.net.WorldOfWarcraft.Models;
public class Profile(IApiRequestExecutor api) : IProfile
{
    public ICharacterEquipment CharacterEquipment { get; } = new CharacterEquipment(api);
}

public interface IProfile
{
    ICharacterEquipment CharacterEquipment { get; }
}