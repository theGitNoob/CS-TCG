namespace Cards;

public enum CardType
{
    Hero = 0,
    Item = 1
}

public class SimpleCard : ICard
{
    public CardType Type { set; get; }
    public int Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string Name { set; get; }
    public string Description { set; get; }

    public SimpleCard(string name, string description, string effect)
    {
        Name = name;
        Description = description;

    }
}
public class ItemCard : SimpleCard
{

    public ItemCard(string name, string description, string effect) : base(name, description, effect)
    {
        Type = CardType.Item;

    }
}
public class HeroCard : SimpleCard
{
    public HeroCard(string name, string description, string effect) : base(name, description, effect)
    {
        Type = CardType.Hero;
    }
}