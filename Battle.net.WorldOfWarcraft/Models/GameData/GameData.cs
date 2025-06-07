namespace Battle.net.WorldOfWarcraft.Models;

public class GameData(IApiRequestExecutor api) : IGameData
{
    public IAchievementFunction Achievement { get; } = new AchievementFunction(api);
    public IAuctionHouseFunction AuctionHouse { get; } = new AuctionHouseFunction(api);
    public IAzeriteEssenceFunction AzeriteEssence { get; } = new AzeriteEssenceFunction(api);
    public IConnectedRealmFunction ConnectedRealms { get; } = new ConnectedRealmFunction(api);
    public ICovenantFunction Covenant { get; } = new CovenantFunction(api);
    public ICreatureFunction Creature { get; } = new CreatureFunction(api);
    public IGuildCrestFunction GuildCrest { get; } = new GuildCrestFunction(api);
    public IHeirloomFunction Heirloom { get; } = new HeirloomFunction(api);
    public IItemFunction Item { get; } = new ItemFunction(api);
    public IJournalFunction Journal { get; } = new JournalFunction(api);
    public IMediaSearchFunction MediaSearch { get; } = new MediaSearchFunction(api);
    public IModifiedCraftingFunction ModifiedCrafting { get; } = new ModifiedCraftingFunction(api);
    public IMountFunction Mount { get; } = new MountFunction(api);
    public IMythicKeystoneAffixFunction MythicKeystoneAffix { get; } = new MythicKeystoneAffixFunction(api);
    public IMythicKeystoneDungeonFunction MythicKeystoneDungeon { get; } = new MythicKeystoneDungeonFunction(api);
    public IMythicKeystoneLeaderboardFunction MythicKeystoneLeaderboard { get; } = new MythicKeystoneLeaderboardFunction(api);
    public IMythicRaidLeaderboardFunction MythicRaidLeaderboard { get; } = new MythicRaidLeaderboardFunction(api);
    public IPetFunction Pet { get; } = new PetFunction(api);
    public IPlayableClassFunction PlayableClass { get; } = new PlayableClassFunction(api);
    public IPlayableRaceFunction PlayableRace { get; } = new PlayableRaceFunction(api);
    public IPlayableSpecializationFunction PlayableSpecialization { get; } = new PlayableSpecializationFunction(api);
    public IPowerTypeFunction PowerType { get; } = new PowerTypeFunction(api);
    public IProfessionFunction Profession { get; } = new ProfessionFunction(api);
    public IPvPSeasonFunction PvPSeason { get; } = new PvPSeasonFunction(api);
    public IPvPTierFunction PvPTier { get; } = new PvPTierFunction(api);
    public IQuestFunction Quest { get; } = new QuestFunction(api);
    public IRealmFunction Realm { get; } = new RealmFunction(api);
    public IRegionFunction Region { get; } = new RegionFunction(api);
    public IReputationsFunction Reputation { get; } = new ReputationsFunction(api);
    public ISpellFunction Spell { get; } = new SpellFunction(api);
    public ITalentFunction Talent { get; } = new TalentFunction(api);
    public ITechTalentFunction TechTalent { get; } = new TechTalentFunction(api);
    public ITitleFunction Title { get; } = new TitleFunction(api);
    public IToyFunction Toy { get; } = new ToyFunction(api);
    public IWoWTokenFunction WowToken { get; } = new WoWTokenFunction(api);
}

public interface IGameData
{
    IAchievementFunction Achievement { get; }
    IAuctionHouseFunction AuctionHouse { get; }
    IAzeriteEssenceFunction AzeriteEssence { get; }
    IConnectedRealmFunction ConnectedRealms { get; }
    ICovenantFunction Covenant { get; }
    ICreatureFunction Creature { get; }
    IGuildCrestFunction GuildCrest { get; }
    IHeirloomFunction Heirloom { get; }
    IItemFunction Item { get; }
    IJournalFunction Journal { get; }
    IMediaSearchFunction MediaSearch { get; }
    IModifiedCraftingFunction ModifiedCrafting { get; }
    IMountFunction Mount { get; }
    IMythicKeystoneAffixFunction MythicKeystoneAffix { get; }
    IMythicKeystoneDungeonFunction MythicKeystoneDungeon { get; }
    IMythicKeystoneLeaderboardFunction MythicKeystoneLeaderboard { get; }
    IMythicRaidLeaderboardFunction MythicRaidLeaderboard { get; }
    IPetFunction Pet { get; }
    IPlayableClassFunction PlayableClass { get; }
    IPlayableRaceFunction PlayableRace { get; }
    IPlayableSpecializationFunction PlayableSpecialization { get; }
    IPowerTypeFunction PowerType { get; }
    IProfessionFunction Profession { get; }
    IPvPSeasonFunction PvPSeason { get; }
    IPvPTierFunction PvPTier { get; }
    IQuestFunction Quest { get; }
    IRealmFunction Realm { get; }
    IRegionFunction Region { get; }
    IReputationsFunction Reputation { get; }
    ISpellFunction Spell { get; }
    ITalentFunction Talent { get; }
    ITechTalentFunction TechTalent { get; }
    ITitleFunction Title { get; }
    IToyFunction Toy { get; }
    IWoWTokenFunction WowToken { get; }
}