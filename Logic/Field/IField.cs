public interface IField
{
    int maxHeroCards { get; set; }

    int maxItemCards { get; set; }

    void PlaceCard(ICard card);

    void RemoveCard(ICard card);

}