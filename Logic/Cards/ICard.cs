using Cards;
public interface ICard
{
    CardType Type { get; set; }
    int Id { get; set; }
    String Name { get; set; }
    String Description { get; set; }

}