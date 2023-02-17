using System.Text.Json.Serialization;

namespace Cards;

/// <summary>
/// Represents a hero card
/// </summary>
public class ItemCard : SimpleCard
{
    /// <summary>
    /// The hero that has this item
    /// </summary>
    [JsonIgnore]
    public HeroCard? Hero { get; private set; }

    /// <summary>
    /// Creates a new item
    /// </summary>
    /// <param name="name">The name of the item</param>
    /// <param name="description">The description of the item</param>
    /// <param name="effectString">The effect of the item</param>
    /// <exception cref="ArgumentNullException">Thrown when the name, description or effect is null</exception>
    /// <returns>A new item</returns>
    public ItemCard(string name, string description, string effectString) : base(name, description, effectString)
    {
        Type = CardType.Item;

        Hero = null;
    }

    
    [JsonConstructor]
    public ItemCard(string name, string description, Effect.Effect effect) : base(name, description, effect)
    {
        Type = CardType.Item;

        Hero = null;
    }

    /// <summary>
    /// Equips the item to a hero
    /// </summary>
    /// <param name="hero">The hero to equip the item</param>
    /// <exception cref="Exception">Thrown when the item is already equipped to a hero</exception>
    public void EquipToHero(HeroCard hero)
    {
        if (Hero != null)
        {
            throw new Exception("Cannot equip the same item to two or more heroes");
        }

        Hero = hero;
    }


    //Removes the item
    /// <summary>
    /// Removes the item from the hero
    /// </summary>
    /// <exception cref="Exception">Thrown when the item is not equipped to a hero</exception>
    public void RemoveFromHero()
    {
        if (Hero == null)
        {
            throw new Exception("Can't remove the item from a hero if not equipped");
        }

        Hero = null;
    }

    public override bool Equals(object? obj)
    {
        if (!(obj is ItemCard)) return false;

        var item = obj as ItemCard;

        return item!.Id == Id;
    }

    public override int GetHashCode()
    {
        return this.Id;
    }
}