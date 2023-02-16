namespace Cards;

/// <summary>
/// Represents a hero
/// </summary>
public class HeroCard : SimpleCard
{
    /// <summary>
    /// The items that the hero has equipped
    /// </summary>
    public List<ItemCard> Items { get; }

    int _attack;

    int _defense;

    /// <summary>
    /// The attack of the hero
    /// </summary>
    public int Attack { get => this._attack; set => this._attack = value <= 0 ? 0 : value;
    }

    /// <summary>
    /// The defense of the hero
    /// </summary>
    public int Defense { get => this._defense; set => this._defense = value <= 0 ? 0 : value;
    }

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
    public HeroCard(string name, int attack, int defense, string description, Effect.Effect effect) : base(name, description, effect)
    {
        Type = CardType.Hero;

        this.Attack = attack;

        this.Defense = defense;

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
    /// Sets the hero attack
    /// </summary>
    /// <param name="attack">The attack of the hero</param>
    /// <exception cref="ArgumentException">Thrown when the attack is not between 0 and 100</exception>
    public void UpdateAttack(int attack)
    {
        if (attack < 0 || attack > 100)
            throw new ArgumentException("Attack must be a value between 0 and 100");
        Attack = attack;
    }


    /// <summary>
    /// Sets the hero defense
    /// </summary>
    /// <param name="defense">The defense of the hero</param>
    /// <exception cref="ArgumentException">Thrown when the defense is not between 0 and 100</exception>
    public void UpdateDefense(int defense)
    {
        if (defense < 0 || defense > 100)
            throw new ArgumentException("Defense must be between 0 and 100");

        this.Defense = defense;
    }

    /// <summary>
    /// Reimplementing the equals method
    /// </summary>
    /// <param name="obj">The object to compare</param>
    /// <returns>True if the objects are equal, false otherwise</returns>
    public override bool Equals(object? obj)
    {
        if (!(obj is HeroCard)) return false;

        var hero = obj as HeroCard;

        return hero!.Id == Id;
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