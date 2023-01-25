using Cards;
public interface ICard
{
    CardType Type { get; }

    int Id { get; init; }

    int GenRandId();

    String Name { get; }

    String Description { get; }

    String Effect { get; }


}