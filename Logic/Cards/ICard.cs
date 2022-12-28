using Cards;
public interface ICard
{
    CardType Type { get; set; }
    int Id { get; init; }

    int GenRandId();
    String Name { get; set; }
    String Description { get; set; }

    String Effect { get; set; }


}