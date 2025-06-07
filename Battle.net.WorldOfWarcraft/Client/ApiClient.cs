namespace Battle.net.WorldOfWarcraft;

public class ApiClient(IApiRequestExecutor api)
{
    public Client US { get; } = new Client(api, Region.US, Locale.en_US);
    public Client EU { get; } = new Client(api, Region.EU, Locale.en_GB);
    public Client KR { get; } = new Client(api, Region.KR, Locale.ko_KR);
    public Client TW { get; } = new Client(api, Region.TW, Locale.zh_TW);
    public Client CN { get; } = new Client(api, Region.CN, Locale.zh_CN);
}

public class Client(IApiRequestExecutor api, Region region, Locale locale) : IClient
{    
    public IGameData GameData { get; } = new GameData(api);
    public IProfile Profile { get; } = new Profile(api);

}

public interface IClient
{
    IGameData GameData { get; }
    IProfile Profile { get; }
}