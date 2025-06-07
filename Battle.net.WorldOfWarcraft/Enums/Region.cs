using System.ComponentModel;

namespace Battle.net.WorldOfWarcraft.Models;

public enum Region
{
    [Description("North America")]
    US,
    [Description("Europe")]
    EU,
    [Description("Korea")]
    KR,
    [Description("Taiwan")]
    TW,
    [Description("China")]
    CN
}