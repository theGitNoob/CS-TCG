using Cards;
using Deck;

namespace Player;

/// <summary>
/// Class that represents an AI Player
/// </summary>
public class AiPlayer : SimplePlayer
{
    /// <summary>
    /// Creates a new AI Player
    /// </summary>
    /// <param name="name"> Name of the player</param>
    /// <param name="hp">Initial life of the player</param>
    /// <param name="maxHeroCards">Max number of Hero cards on the Field</param>
    /// <param name="maxItemCards">Max number of Item cards on the Field</param>
    /// <param name="deck">Deck of the player</param>
    public AiPlayer(string name, int hp, int maxHeroCards, int maxItemCards, SimpleDeck deck) : base(name, hp,
        maxHeroCards, maxItemCards, deck)
    {
    }

    /// <summary>
    /// Plays the AI Player using a greedy strategy
    /// </summary>
    /// <exception cref="Exception">Thrown when the enemy is not set</exception>
    public override void Play(bool canAttack)
    {
        if (Enemy == null) throw new Exception("Enemy not set");

        if (CanInvokeHero())
        {
            HeroCard hero = GetHeroWithHigherAttack(Hand);

            //Invoke a hero
            InvokeHero(hero);
        }

        if (canAttack && HasHeroOnField())
        {
            //AI strongest Hero
            HeroCard strongestHero = GetHeroWithHigherAttack(PlayerField.HeroZone);

            //Attack the enemy hero with the lowest defense
            if (Enemy.HasHeroOnField())
            {
                //Enemy weakest hero
                HeroCard enemyHero = GetHeroWithLowestDefense(Enemy.PlayerField.HeroZone);

                if (strongestHero.Attack >= enemyHero.Defense)
                    AttackHero(strongestHero, enemyHero);
            }
            else
            {
                DirectAttack(strongestHero);
            }
        }


        //Equip a random item to a random hero
        if (CanEquipItem())
        {
            HeroCard hero = GetRandomInvokedHero();
            ItemCard item = GetRandomItem();

            if (item.Hero == null)
                EquipItem(hero, item);
        }


        //Activates all possibly effects
        List<SimpleCard> cards = HeroZone.Concat<SimpleCard>(ItemZone).ToList();

        foreach (SimpleCard card in cards)
        {
            if (card.Effect.CanActivate(this, Enemy))
                card.Effect.Activate(this, Enemy);
        }
    }

    /// <summary>
    /// Gets a random item from the player hand
    /// </summary>
    /// <returns>A random item from the player hand</returns>
    private ItemCard GetRandomItem()
    {
        Random rnd = new Random();

        List<ItemCard> items = Hand.Where(c => c is ItemCard).Cast<ItemCard>().ToList();

        return items[rnd.Next(items.Count)];
    }

    /// <summary>
    /// Gets a random hero from the player field
    /// </summary>
    /// <returns>A random hero from the player field</returns>
    private HeroCard GetRandomInvokedHero()
    {
        //Get a random hero
        Random rnd = new Random();

        return HeroZone[rnd.Next(HeroZone.Count)];
    }


    /// <summary>
    /// Gets the hero with the higher attack given a certain collection
    /// </summary>
    /// <param name="collection">Collection to select hero from</param>
    /// <returns>Hero with the higher attack from the collection</returns>
    private static HeroCard GetHeroWithHigherAttack(IEnumerable<ICard> collection)
    {
        //Filter the hand to get only the heroes
        IEnumerable<HeroCard> heroes = collection.OfType<HeroCard>();

        return heroes.MaxBy(x => x.Attack)!;
    }

    /// <summary>
    /// Gets the hero with the lowest defense given a certain collection
    /// </summary>
    /// <param name="collection">Collection to select hero from</param>
    /// <returns>Hero with the lowest defense from the collection</returns>
    private static HeroCard GetHeroWithLowestDefense(IEnumerable<ICard> collection)
    {
        //Filter the enemy field to get only the heroes
        IEnumerable<HeroCard> heroes = collection.OfType<HeroCard>();

        return heroes.MinBy(x => x.Defense)!;
    }
}