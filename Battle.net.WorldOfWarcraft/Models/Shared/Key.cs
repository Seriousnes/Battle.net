using System.Diagnostics.CodeAnalysis;

namespace Battle.net.WorldOfWarcraft.Models;

public abstract class KeyModel
{
    public Key Key { get; set; }
}

public class Key
{
    [SuppressMessage("Style", "IDE1006:Naming Styles")]
    public string href { get; set; }
}