using System.Text.Json.Serialization;

namespace Game.Cards;

/// <summary>
/// Represents a hero
/// </summary>
public class HeroCard : SimpleCard
{
    /// <summary>
    /// The items that the hero has equipped
    /// </summary>
    [JsonIgnore]
    public List<ItemCard> Items { get; }

    [JsonIgnore] int _attack;

    [JsonIgnore] int _defense;

    /// <summary>
    /// The attack of the hero
    /// </summary>
    public int Attack
    {
        get => _attack;
        set => _attack = value <= 0 ? 0 : value;
    }

    /// <summary>
    /// The defense of the hero
    /// </summary>
    public int Defense
    {
        get => _defense;
        set => _defense = value <= 0 ? 0 : value;
    }

    /// <summary>
    /// Creates a new hero
    /// </summary>
    /// <param name="name">The name of the hero</param>
    /// <param name="attack">The attack of the hero</param>
    /// <param name="defense">The defense of the hero</param>
    /// <param name="description">The description of the hero</param>
    /// <param name="effectString">The effect of the hero</param>
    public HeroCard(string name, int attack, int defense, string description, string effectString) : base(name,
        description, effectString)
    {
        Type = CardType.Hero;

        Attack = attack;

        Defense = defense;

        Items = new List<ItemCard>();
    }

    /// <summary>
    /// Creates a new hero
    /// </summary>
    /// <param name="name">The name of the hero</param>
    /// <param name="attack">The attack of the hero</param>
    /// <param name="defense">The defense of the hero</param>
    /// <param name="description">The description of the hero</param>
    /// <param name="effect">The effect of the hero</param>
    [JsonConstructor]
    public HeroCard(string name, int attack, int defense, string description, Effect.Effect effect) : base(name,
        description, effect)
    {
        Type = CardType.Hero;

        Attack = attack;

        Defense = defense;

        Items = new List<ItemCard>();
    }

    /// <summary>
    /// Equips the given item to the hero
    /// </summary>
    /// <param name="item">The item to equip</param>
    public void EquipItem(ItemCard item)
    {
        Items.Add(item);
    }

    /// <summary>
    /// Removes the given item from the hero
    /// </summary>
    /// <param name="item">The item to remove</param>
    /// <exception cref="Exception">Thrown when the item is not equipped to the hero</exception>
    public void RemoveItem(ItemCard item)
    {
        if (Items.Remove(item) == false)
            throw new Exception($"The item {item.Name} is not equipped to the hero {Name}");
    }

    /// <summary>
    /// Removes all items from hero
    /// </summary>
    public void RemoveAllItems()
    {
        Items.Clear();
    }

    /// <summary>
    /// Reimplementing the equals method
    /// </summary>
    /// <param name="obj">The object to compare</param>
    /// <returns>True if the objects are equal, false otherwise</returns>
    public override bool Equals(object? obj)
    {
        if (obj is not HeroCard hero) return false;

        return hero.Id == Id;
    }

    /// <summary>
    /// Reimplementing the `GetHashCode` method
    /// </summary>
    /// <returns>The hashcode of the object</returns>
    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}