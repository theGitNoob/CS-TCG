using Habilitie;

namespace Cards;
public interface ICard
{
    CardType Type { get; }

    int Id { get; init; }

    int GenRandId();

    String Name { get; }

    String Description { get; }

    Effect Effect { get; }


}
/// <summary>
/// Represents a card type
/// </summary>
public enum CardType
{
    Hero = 0,
    Item = 1
}

/// <summary>
/// Represents a card
/// </summary>
public class SimpleCard : ICard
{
    /// <summary>
    /// The type of the card
    /// </summary>
    public CardType Type { get; init; }

    /// <summary>
    /// The id of the card
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The name of the card
    /// </summary>
    public string Name { set; get; }

    /// <summary>
    /// The description of the card
    /// </summary>
    public string Description { get; init; }

    /// <summary>
    /// The effect of the card
    /// </summary>
    public Effect Effect { get; init; }

    /// <summary>
    /// Creates a new card
    /// </summary>
    /// <param name="name">The name of the card</param>
    /// <param name="description">The description of the card</param>
    /// <param name="effect">The effect of the card</param>
    /// <exception cref="ArgumentNullException">Thrown when the name, description or effect is null</exception>
    /// <returns>A new card</returns>
    public SimpleCard(string name, string description, Effect effect)
    {
        if (name == null) throw new ArgumentNullException(nameof(name));
        if (description == null) throw new ArgumentNullException(nameof(description));
        if (effect == null) throw new ArgumentNullException(nameof(effect));

        Name = name;

        Description = description;

        Effect = effect;

        Id = GenRandId();
    }

    /// <summary>
    /// Generates a random id
    /// </summary>
    /// <returns>A random id</returns>
    public int GenRandId() => Guid.NewGuid().ToString().GetHashCode();
}

/// <summary>
/// Represents a hero card
/// </summary>
public class ItemCard : SimpleCard
{
    /// <summary>
    /// The hero that has this item
    /// </summary>
    private HeroCard? Hero;

    /// <summary>
    /// Creates a new item
    /// </summary>
    /// <param name="name">The name of the item</param>
    /// <param name="description">The description of the item</param>
    /// <param name="effect">The effect of the item</param>
    /// <exception cref="ArgumentNullException">Thrown when the name, description or effect is null</exception>
    /// <returns>A new item</returns>
    public ItemCard(string name, string description, Effect effect) : base(name, description, effect)
    {
        Type = CardType.Item;

        Hero = null;
    }

    /// <summary>
    /// Equips the item to a hero
    /// </summary>
    /// <param name="hero">The hero to equip the item</param>
    /// <exception cref="Exception">Thrown when the item is already equiped to a hero</exception>
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
    /// <exception cref="Exception">Thrown when the item is not equiped to a hero</exception>
    public void RemoveFromHero()
    {
        if (Hero == null)
        {
            throw new Exception("Can't remove the item from a hero if not equiped");
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

/// <summary>
/// Represents a hero
/// </summary>
public class HeroCard : SimpleCard
{
    /// <summary>
    /// The items that the hero has equiped
    /// </summary>
    private List<ItemCard> _items;


    /// <summary>
    /// The attack of the hero
    /// </summary>
    public int Attack { get; set; }

    /// <summary>
    /// The defense of the hero
    /// </summary>
    public int Defense { get; set; }

    /// <summary>
    /// Creates a new hero
    /// </summary>
    /// <param name="name">The name of the hero</param>
    /// <param name="attack">The attack of the hero</param>
    /// <param name="defense">The defense of the hero</param>
    /// <param name="description">The description of the hero</param>
    /// <param name="effect">The effect of the hero</param>
    /// <exception cref="ArgumentNullException">Thrown when the name, description or effect is null</exception>
    /// <returns>A new hero</returns>
    public HeroCard(string name, int attack, int defense, string description, Effect effect) : base(name, description, effect)
    {
        Type = CardType.Hero;

        _items = new List<ItemCard>();
    }


    /// <summary>
    /// Equips the given item to the hero
    /// </summary>
    /// <param name="item">The item to equip</param>
    public void EquipItem(ItemCard item)
    {
        _items.Add(item);
    }

    //Removes the item from the equiped items
    /// <summary>
    /// Removes the given item from the hero
    /// </summary>
    /// <param name="item">The item to remove</param>
    ///<exception cref="ArgumentException">Thrown when the item is not equiped to the hero</exception>
    public void RemoveItem(ItemCard item)
    {
        _items.Remove(item);
    }


    ///<summary>
    ///Sets the hero attack
    ///</summary>
    ///<param name="attack">The attack of the hero</param>
    ///<exception cref="ArgumentException">Thrown when the attack is not between 0 and 100</exception>
    public void UpdateAttack(int attack)
    {
        if (attack < 0 || attack > 100)
            throw new ArgumentException("Attack must be a value between 0 and 100");
        Attack = attack;
    }


    ///<summary>
    ///Sets the hero defense
    ///</summary>
    ///<param name="defense">The defense of the hero</param>
    ///<exception cref="ArgumentException">Thrown when the defense is not between 0 and 100</exception>
    public void UpdateDefense(int defense)
    {
        if (defense < 0 || defense > 100)
            throw new ArgumentException("Defense must be between 0 and 100");

        this.Defense = defense;
    }

    ///<summary>
    ///Reimplementing the equals method
    ///</summary>
    ///<param name="obj">The object to compare</param>
    ///<returns>True if the objects are equal, false otherwise</returns>
    public override bool Equals(object? obj)
    {
        if (!(obj is HeroCard)) return false;

        var hero = obj as HeroCard;

        return hero!.Id == Id;
    }

    ///<summary>
    ///Reimplementing the gethashcode method
    ///</summary>
    ///<returns>The hashcode of the object</returns>
    public override int GetHashCode()
    {
        return this.Id;
    }
}