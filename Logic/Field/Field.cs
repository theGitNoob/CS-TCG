namespace Field;
using Cards;
public class SimpleField : IField
{
    //Holds the HeroZone Cards
    public List<HeroCard> HeroZone { private set; get; }

    //Holds the ItemZone Cards
    public List<ItemCard> ItemZone { private set; get; }

    //Holds the Cementery Zone Cards
    public List<SimpleCard> CementeryZone { private set; get; }

    //Max number of Hero cards on the Field
    public int maxHeroCards { private set; get; }

    //Max number of Item cards on the Field
    public int maxItemCards { private set; get; }


    /// <summary>
    /// Creates a new Field with the given max number of cards
    /// </summary>
    /// <param name="maxHeroCards">Max number of Hero cards on the Field</param>
    /// <param name="maxItemCards">Max number of Item cards on the Field</param>
    /// <returns>A new Field</returns>
    public SimpleField(int maxHeroCards, int maxItemCards)
    {
        this.maxHeroCards = maxHeroCards;
        this.maxItemCards = maxItemCards;

        HeroZone = new List<HeroCard>();
        ItemZone = new List<ItemCard>();

        CementeryZone = new List<SimpleCard>();
    }


    /// <summary>
    /// Sends a given hero to the cementery
    /// and remove it from the hero zone along 
    /// with it equiped items
    /// </summary>
    /// <param name="heroToDelete">Hero to be removed</param>
    /// <returns>True if the hero was removed, false otherwise</returns>
    public bool RemoveHero(HeroCard heroToDelete)
    {
        foreach (HeroCard hero in HeroZone)
        {
            if (hero.Equals(heroToDelete))
            {
                foreach (ItemCard items in hero.Items)
                {
                    RemoveItem(items);
                }

                hero.RemoveAllItems();
                HeroZone.Remove(heroToDelete);
                CementeryZone.Add(heroToDelete);

                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Sends a given item to the cementery
    /// and remove it from the item zone
    /// </summary>
    /// <param name="itemToDelete">Item to be removed</param>
    /// <returns>True if the item was removed, false otherwise</returns>
    public bool RemoveItem(ItemCard itemToDelete)
    {
        foreach (ItemCard item in ItemZone)
        {
            if (item.Equals(itemToDelete))
            {
                ItemZone.Remove(item);
                item.RemoveFromHero();
                CementeryZone.Add(item);

                return true;
            }
        }

        return false;
    }


    /// <summary>
    /// Places a given hero on the field
    /// </summary>
    /// <param name="hero">Hero to be placed</param>
    /// <returns>True if the hero was placed, false otherwise</returns>
    public bool PlaceHero(HeroCard hero)
    {
        if (CanInvokeHero())
        {
            HeroZone.Add(hero);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Places a given item on the field
    /// </summary>
    /// <param name="item">Item to be placed</param>
    /// <returns>True if the item was placed, false otherwise</returns>
    public bool PlaceItem(ItemCard item)
    {
        if (CanEquipItem())
        {
            ItemZone.Add(item);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Checks if a hero can be placed on the field
    /// </summary>
    /// <returns>True if a hero can be placed, false otherwise</returns>
    public bool CanInvokeHero()
    {
        return HeroZone.Count < maxHeroCards;
    }

    /// <summary>
    /// Checks if an item can be placed on the field
    /// </summary>
    /// <returns>True if an item can be placed, false otherwise</returns>
    public bool CanEquipItem()
    {
        return ItemZone.Count < maxItemCards;
    }

    /// <summary>
    /// Checks if the hero is currently on the Player Field
    /// </summary>
    /// <param name="heroName"> The name of the hero to search for</param>
    /// <returns>True if hero is on Field, false otherwise</returns>
    public bool IsHeroOnField(string heroName)
    {
        return HeroZone.Exists(hero => hero.Name == heroName);
    }

    /// <summary>
    /// Checks if the item is currently on the Player Field
    /// </summary>
    /// <param name="itemName"> The name of the item to search for</param>
    /// <returns>True if item is on Field, false otherwise</returns>
    public bool IsItemOnField(string itemName)
    {
        return ItemZone.Exists(item => item.Name == itemName);
    }

    /// <summary>
    /// Returns a hero card with the given name
    /// </summary>
    /// <param name="heroName">Name of the hero to be found</param>
    /// <exception cref="KeyNotFoundException">Thrown when the hero is not found</exception>
    /// <returns>The hero with the given name</returns>
    public HeroCard GetHeroCard(string heroName)
    {
        foreach (HeroCard hero in HeroZone)
        {
            if (hero.Name == heroName)
            {
                return hero;
            }
        }
        throw new KeyNotFoundException($"Card {heroName} was not found");
    }

    public bool TryGetHeroCard(string heroName, out HeroCard? hero)
    {
        if (IsHeroOnField(heroName))
        {
            hero = GetHeroCard(heroName);
            return true;
        }

        hero = null;

        return false;
    }

    public bool TryGetItemCard(string itemName, out ItemCard? item)
    {
        if (IsItemOnField(itemName))
        {
            item = GetItemCard(itemName);
            return true;
        }

        item = null;
        return false;
    }


    public bool IsHeroAt(int position)
    {
        if (position >= HeroZone.Count) return false;

        return true;
    }

    public bool IsItemAt(int position)
    {
        if (position >= ItemZone.Count) return false;

        return true;
    }

    public HeroCard GetHeroCard(int position)
    {
        if (position >= HeroZone.Count) throw new ArgumentOutOfRangeException(nameof(position));

        return HeroZone[position];

    }
    public ItemCard GetItemCard(int position)
    {
        if (position >= ItemZone.Count) throw new ArgumentOutOfRangeException(nameof(position));

        return ItemZone[position];

    }
    /// <summary>
    /// Returns an item card with the given name
    /// </summary>
    /// <param name="cardName">Name of the item to be found</param>
    /// <exception cref="Exception">Thrown when the item is not found</exception>
    /// <returns>The item with the given name</returns>
    public ItemCard GetItemCard(string cardName)
    {
        foreach (ItemCard item in ItemZone)
        {
            if (item.Name == cardName)
            {
                return item;
            }
        }
        throw new Exception($"Card {cardName} was not found");
    }
}
