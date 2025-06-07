namespace Battle.net.WorldOfWarcraft.Models;

public class ConnectedRealmSearchModel : MediaDocument
{
    public ConnectedRealmSearchResult[] Results { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int MaxPageSize { get; set; }
    public int PageCount { get; set; }
    public int ResultCountCurrent { get; set; }
    public int ResultCountTotal { get; set; }
}

public class ConnectedRealmSearchResult
{
    public ConnectedRealmSearchData Data { get; set; }
}

public class ConnectedRealmSearchData
{
    public int Id { get; set; }
    public Status Status { get; set; }
    public Population Population { get; set; }
    public Realm[] Realms { get; set; }
}