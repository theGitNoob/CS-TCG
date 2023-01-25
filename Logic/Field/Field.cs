namespace Field;
using Cards;
public class SimpleField : IField
{
    //Holds the HeroZone Cards
    public List<HeroCard> HeroZone { private set; get; }

    //Holds the ItemZone Cards
    public List<ItemCard> ItemZone { private set; get; }

    //Holds the CementeryZone Cards
    public List<SimpleCard> CementeryZone { private set; get; }

    //Max number of Hero cards on the Field
    public int maxHeroCards { private set; get; }

    //Max number of Item cards on the Field
    public int maxItemCards { private set; get; }


    ///<summary>
    ///Creates a new Field with the given max number of cards
    ///</summary>
    ///<param name="maxHeroCards">Max number of Hero cards on the Field</param>
    ///<param name="maxItemCards">Max number of Item cards on the Field</param>
    ///<returns>A new Field</returns>
    public SimpleField(int maxHeroCards, int maxItemCards)
    {
        this.maxHeroCards = maxHeroCards;
        this.maxItemCards = maxItemCards;
        HeroZone = new List<HeroCard>();
        ItemZone = new List<ItemCard>();
        CementeryZone = new List<SimpleCard>();
    }


    ///<summary>
    ///Sends a given hero to the cementery
    ///and remove it from the hero zone along 
    ///with it equiped items
    /// </summary>
    ///<param name="hero">Hero to be removed</param>
    ///<returns>True if the hero was removed, false otherwise</returns>
    public bool RemoveHero(HeroCard hero)
    {
        throw new NotImplementedException();

    }

    ///<summary>
    ///Sends a given item to the cementery
    ///and remove it from the item zone
    /// </summary>
    ///<param name="item">Item to be removed</param>
    ///<returns>True if the item was removed, false otherwise</returns>
    public bool RemoveItem(ItemCard item)
    {
        throw new NotImplementedException();
    }


    ///<summary>
    ///Places a given hero on the field
    /// </summary>
    ///<param name="hero">Hero to be placed</param>
    ///<returns>True if the hero was placed, false otherwise</returns>
    public bool PlaceHero(HeroCard hero)
    {
        if (CanInvokeHero())
        {
            HeroZone.Add(hero);
            return true;
        }
        return false;
    }

    ///<summary>
    ///Places a given item on the field
    /// </summary>
    ///<param name="item">Item to be placed</param>
    ///<returns>True if the item was placed, false otherwise</returns>
    public bool PlaceItem(ItemCard item)
    {
        if (CanEquipItem())
        {
            ItemZone.Add(item);
            return true;
        }
        return false;
    }

    ///<summary>
    ///Checks if a hero can be placed on the field
    ///</summary>
    ///<returns>True if a hero can be placed, false otherwise</returns>
    public bool CanInvokeHero()
    {
        return HeroZone.Count < maxHeroCards;
    }

    ///<summary>
    ///Checks if an item can be placed on the field
    ///</summary>
    ///<returns>True if an item can be placed, false otherwise</returns>
    public bool CanEquipItem()
    {
        return ItemZone.Count < maxItemCards;
    }
}
