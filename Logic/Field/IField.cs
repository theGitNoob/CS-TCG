using Cards;

namespace Field;

public interface IField
{
    int maxHeroCards { get; }

    int maxItemCards { get; }

    bool PlaceHero(HeroCard hero);

    bool PlaceItem(ItemCard item);

    bool RemoveHero(HeroCard hero);

    bool RemoveItem(ItemCard item);

    bool CanInvokeHero();

    bool CanEquipItem();

    HeroCard GetHeroCard(string name); 

    ItemCard GetItemCard(string name); 
}