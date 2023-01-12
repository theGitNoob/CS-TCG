namespace Cards;

public enum CardType
{
    Hero = 0,
    Item = 1
}

public class SimpleCard : ICard
{
    public CardType Type { set; get; }
    public int Id { get; init; }

    public string Name { set; get; }
    public string Description { set; get; }

    public string Effect { get; set; }

    public SimpleCard(string name, string description, string effect)
    {
        Name = name;
        Description = description;
        Effect = "";

        Id = GenRandId();

    }

    public int GenRandId() => Guid.NewGuid().ToString().GetHashCode();
}
public class ItemCard : SimpleCard
{
    //Holds the Hero to which the item is equiped
    private HeroCard? Hero;
    public ItemCard(string name, string description, string effect) : base(name, description, effect)
    {
        Type = CardType.Item;
        Hero = null;

    }

    //Add the equipped hero with this item
    public void EquipToHero(HeroCard hero)
    {
        if (Hero != null)
        {
            throw new Exception("Cannot equip the same item to two or more heroes");
        }

        Hero = hero;
    }


    //Removes the item
    public void RemoveFromHero()
    {
        if (Hero == null)
        {
            throw new Exception("Can't remove the item from a hero if not equiped");
        }

        Hero = null;

    }



}
public class HeroCard : SimpleCard
{
    //Holds the equiped items of the hero
    private List<ItemCard> _items;


    //Hero attack
    public int Attack { get; protected set; }

    //Hero defense
    public int Defense { get; protected set; }


    //Hero Mana
    public int Mana { get; protected set; }

    public HeroCard(string name, string description, string effect) : base(name, description, effect)
    {
        Type = CardType.Hero;

        _items = new List<ItemCard>();
    }


    //Equips the given item to the hero
    public void EquipItem(ItemCard item)
    {
        _items.Add(item);
    }

    //Removes the item from the equiped items
    public void RemoveItem(ItemCard item)
    {
        _items.Remove(item);
    }


    ///<summary>
    ///Sets the hero attack
    ///</summary>
    public void UpdateAttack(int attack)
    {
        if (attack < 0 || attack > 100)
            throw new ArgumentException("Attack must be a value between 0 and 100");
        Attack = attack;
    }


    ///<summary>
    ///Sets the hero defense
    ///</summary>

    public void UpdateDefense(int defense)
    {
        if (defense < 0 || defense > 100)
            throw new ArgumentException("Defense must be between 0 and 100");

        this.Defense = defense;
    }

    ///<summary>
    ///Sets the hero mana
    ///</summary>

    public void UpdateMana(int mana)
    {
        if (mana < 0)
            throw new ArgumentException("Mana can't be negative");

        this.Mana = mana;
    }
}