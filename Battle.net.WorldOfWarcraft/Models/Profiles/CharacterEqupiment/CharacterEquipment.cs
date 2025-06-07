using System.Text.Json.Serialization;

namespace Battle.net.WorldOfWarcraft.Models.Profiles;

public class CharacterEquipmentModel : MediaDocument
{
    public Character Character { get; set; }
    [JsonPropertyName("equipped_items")]
    public List<EquippedItem> EquippedItems { get; set; }
}

public class Character : KeyModel
{
    public string Name { get; set; }
    public int Id { get; set; }
    public Realm Realm { get; set; }
}

public class Realm : KeyModel
{
    public string Name { get; set; }
    public int Id { get; set; }
    public string Slug { get; set; }
}

public class EquippedItem
{
    public Item Item { get; set; }
    public Slot Slot { get; set; }
    public int Quantity { get; set; }
    public int Context { get; set; }
    [JsonPropertyName("bonus_list")]
    public List<int> BonusList { get; set; }
    public Quality Quality { get; set; }
    public string Name { get; set; }
    [JsonPropertyName("Modified_appearance_id")]
    public int ModifiedAppearanceId { get; set; }
    public Media Media { get; set; }
    [JsonPropertyName("Item_class")]
    public ItemClass ItemClass { get; set; }
    [JsonPropertyName("Item_subclass")]
    public ItemSubclass ItemSubclass { get; set; }
    [JsonPropertyName("Inventory_type")]
    public BaseType InventoryType { get; set; }
    public BaseType Binding { get; set; }
    public Armor Armor { get; set; }
    public List<Stat> Stats { get; set; }
    [JsonPropertyName("sell_price")]
    public SellPrice SellPrice { get; set; }
    public Requirements Requirements { get; set; }
    public Level Level { get; set; }
    public Transmog Transmog { get; set; }
    public Durability Durability { get; set; }
    [JsonPropertyName("name_description")]
    public NameDescription NameDescription { get; set; }
    [JsonPropertyName("is_subclass_hidden")]
    public bool IsSubclassHidden { get; set; }
    public List<Enchantment> Enchantments { get; set; }
    public List<Socket> Sockets { get; set; }
    [JsonPropertyName("Unique_equipped")]
    public string UniqueEquipped { get; set; }
    public List<SpellModel> Spells { get; set; }
    public Weapon Weapon { get; set; }
}

public class Slot
{
    public string Type { get; set; }
    public string Name { get; set; }
}

public class Quality
{
    public string Type { get; set; }
    public string Name { get; set; }
}

public class Item : KeyModel
{
    public string Name { get; set; }
}

public class ItemClass : KeyModel
{
    public string Name { get; set; }
}

public class ItemSubclass : KeyModel
{
    public string Name { get; set; }
}

public class BaseType
{
    public string Type { get; set; }
    public string Name { get; set; }
}

public class Armor
{
    public int Value { get; set; }
    public Display Display { get; set; }
}

public abstract class BaseDisplay
{
    [JsonPropertyName("display_string")]
    public string Display { get; set; }
}

public abstract class BaseValueDisplay<T> : BaseDisplay
{
    public T Value { get; set; }
}

public class Display : BaseDisplay
{
    public Color Color { get; set; }
}

public class Color
{
    public int R { get; set; }
    public int G { get; set; }
    public int B { get; set; }
    public decimal A { get; set; }
}

public class SellPrice
{
    public int Value { get; set; }
    [JsonPropertyName("display_strings")]
    public DisplayStrings DisplayStrings { get; set; }
}

public class DisplayStrings
{
    public string Header { get; set; }
    public string Gold { get; set; }
    public string Silver { get; set; }
    public string Copper { get; set; }
}

public class Requirements
{
    public Level Level { get; set; }
}

public class Level : BaseValueDisplay<int>
{
}

public class Transmog : BaseDisplay
{
    public Item Item { get; set; }
    [JsonPropertyName("item_modified_appearance_id")]
    public int ItemModifiedAppearanceId { get; set; }
}


public class Durability : BaseValueDisplay<int>
{
}

public class NameDescription : BaseDisplay
{
    public Color Color { get; set; }
}

public class Weapon
{
    public Damage Damage { get; set; }
    [JsonPropertyName("attack_speed")]
    public AttackSpeed AttackSpeed { get; set; }
    public Dps Dps { get; set; }
}

public class Damage : BaseDisplay
{
    [JsonPropertyName("min_value")]
    public int MinValue { get; set; }
    [JsonPropertyName("man_value")]
    public int MaxValue { get; set; }
    [JsonPropertyName("damage_class")]
    public DamageClass DamageClass { get; set; }
}

public class DamageClass
{
    public string Type { get; set; }
    public string Name { get; set; }
}

public class AttackSpeed : BaseValueDisplay<int>
{
}

public class Dps : BaseValueDisplay<float>
{
}

public class Stat
{
    public Type Type { get; set; }
    public int Value { get; set; }
    public Display Display { get; set; }
    [JsonPropertyName("is_negated")]
    public bool IsNegated { get; set; }
    [JsonPropertyName("is_equip_bonus")]
    public bool IsEquipBonus { get; set; }
}

public class Type
{
    [JsonPropertyName("type")]
    public string Type_ { get; set; }
    public string Name { get; set; }
}

public class Enchantment : BaseDisplay
{
    [JsonPropertyName("source_item")]
    public SourceItem SourceItem { get; set; }

    [JsonPropertyName("enchantment_id")]
    public int EnchantmentId { get; set; }
    [JsonPropertyName("enchantment_slot")]
    public EnchantmentSlot EnchantmentSlot { get; set; }
}

public class SourceItem : KeyModel
{
    public string Name { get; set; }
}

public class EnchantmentSlot
{
    public int Id { get; set; }
    public string Type { get; set; }
}

public class Socket : BaseDisplay
{
    [JsonPropertyName("socket_type")]
    public BaseType SocketType { get; set; }
    public Item Item { get; set; }
    public Media Media { get; set; }
}

public class SpellModel
{
    public Item Spell { get; set; }
    public string Description { get; set; }
    [JsonPropertyName("display_color")]
    public Color Color { get; set; }
}