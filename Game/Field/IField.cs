using Game.Cards;

namespace Field;

public interface IField
{
    bool PlaceItem(ItemCard item);

    bool RemoveHero(HeroCard hero);

    bool RemoveItem(ItemCard item);

    bool CanInvokeHero();

    bool CanEquipItem();

    HeroCard GetHeroCard(string name); 

    ItemCard GetItemCard(string name); 
}