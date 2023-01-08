namespace Field;
public class Field : IField
{
    //Holds the HeroZone Cards
    private List<ICard> HeroZone;

    //Holds the ItemZone Cards
    private List<ICard> ItemZone;

    //Holds the CementeryZone Cards
    private List<ICard> CementeryZone;

    //Max number of Hero cards on the Field
    public int maxHeroCards { set; get; }

    //Max number of Item cards on the Field
    public int maxItemCards { set; get; }


    ///<summary>
    ///Creates a new Field with the given max number of cards
    ///</summary>

    ///<param name="maxHeroCards">Max number of Hero cards on the Field</param>
    ///<param name="maxItemCards">Max number of Item cards on the Field</param>
    ///<returns>A new Field</returns>

    public Field(int maxHeroCards, int maxItemCards)
    {
        this.maxHeroCards = maxHeroCards;
        this.maxItemCards = maxItemCards;
        HeroZone = new List<ICard>();
        ItemZone = new List<ICard>();
        CementeryZone = new List<ICard>();
    }

    ///<summary>
    ///Removes a card from the Field
    ///</summary>
    ///<param name="card">Card to be removed</param>
    public void RemoveCard(ICard card)
    {
        switch (card.Type)
        {
            case Cards.CardType.Hero:
                RemoveHero(card);
                break;
            case Cards.CardType.Item:
                RemoveItem(card);
                break;
        }
    }



    ///<summary>
    ///Sends a given hero to the cementery
    ///and remove it from the hero zone along 
    ///with it equiped items
    /// </summary>
    private void RemoveHero(ICard hero)
    {
        throw new NotImplementedException();

    }


    //Remove a item and delet it from the equiped
    // items of the corresponding Hero
    private void RemoveItem(ICard item)
    {
        throw new NotImplementedException();
    }

    ///<summary>
    ///Place the given card according to its type
    ///</summary>
    public void PlaceCard(ICard card)
    {
        switch (card.Type)
        {
            case Cards.CardType.Hero:
                HeroZone.Add(card);
                break;
            case Cards.CardType.Item:
                ItemZone.Add(card);
                break;
        }
    }

}
