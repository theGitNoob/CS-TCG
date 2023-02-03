namespace Player;

using Cards;
using Field;
using Deck;

///<summary>
///Represents a player in the game
///</summary>
public interface IPlayer
{
    ///<summary>
    ///Player's Name
    ///<summary>
    string Name { get; }

    ///<summary>
    ///The player's health points
    ///</summary>
    int HP { set; get; }

    ///<summary>
    ///The player's deck
    ///</summary>
    SimpleDeck Deck { get; }

    ///<summary>
    ///The player's hand
    ///</summary>
    List<SimpleCard> Hand { get; }

    ///<summary>
    ///The player's field
    ///</summary>
    SimpleField PlayerField { get; }

    ///<summary>
    ///The player's hero zone
    ///</summary>
    List<HeroCard> HeroZone { get; }

    ///<summary>
    ///The player's item zone
    ///</summary>
    List<ItemCard> ItemZone { get; }
}

///<summary>
///Represents a player in the game
///</summary>
public class SimplePlayer : IPlayer
{
    public string Name { get; set; }

    ///<summary>
    ///The player's health points
    ///</summary>
    private int _hp;

    public int HP
    {
        get => _hp;
        set
        {
            if (value < 0)
            {
                _hp = 0;
            }
            else
            {
                _hp = value;
            }
        }
    }
    public List<SimpleCard> Hand { get; protected set; }

    public SimpleDeck Deck { get; protected set; }

    public SimpleField PlayerField { get; protected set; }

    public List<HeroCard> HeroZone => PlayerField.HeroZone;

    public List<ItemCard> ItemZone => PlayerField.ItemZone;


    ///<summary>
    ///Draw a given number of cards from the deck
    ///</summary>
    ///<param name="cnt">Number of cards to be drawn</param>
    public void DrawCards(int cnt)
    {
        List<SimpleCard> cards = Deck.GetTop(cnt);
        Hand.AddRange(cards);
    }

    ///<summary>
    ///Invoke a given hero to the field and removes it from the hand
    ///</summary>
    ///<param name="hero">Hero to be invoked</param>
    public void InvokeHero(HeroCard hero)
    {
        PlayerField.PlaceHero(hero);
        Hand.Remove(hero);
    }

    ///<summary>
    ///Equip a given item to a given hero
    ///</summary>
    ///<param name="hero">Hero that will equip the item</param>
    ///<param name="item">Item to be equipped</param>
    public void EquipItem(HeroCard hero, ItemCard item)
    {
        hero.EquipItem(item);
        item.EquipToHero(hero);
    }

    ///<summary>
    ///Attack a given hero with another given hero
    ///</summary>
    ///<param name="hero">Hero that will attack</param>
    ///<param name="target">Hero that will be attacked</param>
    ///<param name="enemy">Enemy player</param>
    public void AttackHero(HeroCard hero, HeroCard target, SimplePlayer enemy)
    {
        int diff = hero.Attack - target.Defense;
        if (diff < 0)
        {
            UpdateLife(diff);
            PlayerField.RemoveHero(hero);
        }
        else if (diff == 0)
        {
            this.PlayerField.RemoveHero(hero);
            enemy.PlayerField.RemoveHero(target);
        }
        else
        {
            enemy.UpdateLife(-diff);
            enemy.PlayerField.RemoveHero(target);
        }
    }


    ///<summary>
    ///Update the life of the player
    ///</summary>
    ///<param name="value">Value to be added to the life</param>
    public void UpdateLife(int value)
    {
        HP = HP + value;
    }

    ///<summary>
    ///Creates a new Human Player
    ///</summary>
    ///<param name="name"> Name of the player</param>
    ///<param name="hp">Initial life of the player</param>
    ///<param name="maxHeroCards">Max number of Hero cards on the Field</param>
    ///<param name="maxItemCards">Max number of Item cards on the Field</param>
    ///<param name="deck">Deck of the player</param>
    ///<param name="field">Field of the player</param>
    ///<returns>A new Human Player</returns>
    public SimplePlayer(string name, int hp, int maxHeroCards, int maxItemCards, SimpleDeck deck)
    {
        this.Name = name;
        this.HP = hp;
        this.Deck = deck;
        this.Hand = new List<SimpleCard>();
        this.PlayerField = new SimpleField(maxHeroCards, maxItemCards);
    }
}


///<summary>
///Class that represents an AI Player
///</summary>
public class AIPlayer : SimplePlayer
{

    AIPlayer? _enemy { get; set; }

    ///<summary>
    ///Creates a new AI Player
    ///</summary>
    ///<param name="name"> Name of the player</param>
    ///<param name="hp">Initial life of the player</param>
    ///<param name="maxHeroCards">Max number of Hero cards on the Field</param>
    ///<param name="maxItemCards">Max number of Item cards on the Field</param>
    ///<param name="deck">Deck of the player</param>
    ///<returns>A new AI Player</returns>
    public AIPlayer(string name, int hp, int maxHeroCards, int maxItemCards, SimpleDeck deck) : base(name, hp, maxHeroCards, maxItemCards, deck)
    {

    }

    ///<summary>
    ///Sets the enemy of the AI Player
    ///</summary>
    ///<param name="enemy">Enemy of the AI Player</param>
    public void SetEnemy(AIPlayer enemy)
    {
        _enemy = enemy;
    }

    ///<summary>
    ///Plays the AI Player using a greedy strategy
    ///</summary>
    ///<exception cref="Exception">Thrown when the enemy is not set</exception>
    public void Play()
    {
        if (_enemy == null)
        {
            throw new Exception("Enemy not set");
        }

        if (CanInvokeHero())
        {
            HeroCard hero = GetHeroWithHigherAttack(Hand);

            //Invoke a hero
            InvokeHero(hero);
        }

        if (HasHeroOnField())
        {
            //AI strongest Hero
            HeroCard strongestHero = GetHeroWithHigherAttack(PlayerField.HeroZone);

            //Atack the enemy hero with the lowest defense
            if (_enemy.HasHeroOnField())
            {
                //Enemy weakest hero
                HeroCard enemyHero = GetHeroWithLowestDefense(_enemy.PlayerField.HeroZone);

                if (strongestHero.Attack >= enemyHero.Defense)
                    AttackHero(strongestHero, enemyHero, _enemy);
            }
            else
            {
                DirectAttack(strongestHero, _enemy);
            }
        }


        ///Equip a random item to a random hero
        if (CanEquipItem())
        {
            HeroCard hero = GetRandomInvokedHero();
            ItemCard item = GetRandomItem();

            if (item.Hero == null)
                EquipItem(hero, item);
        }


        //Activates all possibly effects
        List<SimpleCard> cards = Enumerable.Concat<SimpleCard>(HeroZone, ItemZone).ToList();

        foreach (SimpleCard card in cards)
        {
            if (card.Effect.CanActivate<SimplePlayer>(this, _enemy))
                card.Effect.Activate<SimplePlayer>(this, _enemy);

        }
    }

    ///<summary>
    ///Gets a random item from the player hand
    ///</summary>
    ///<returns>A random item from the player hand</returns>
    private ItemCard GetRandomItem()
    {
        Random rnd = new Random();

        List<ItemCard> items = Hand.Where(c => c is ItemCard).Cast<ItemCard>().ToList();

        return items[rnd.Next(items.Count)];


    }

    ///<summary>
    ///Gets a random hero from the player field
    ///</summary>
    ///<returns>A random hero from the player field</returns>
    private HeroCard GetRandomInvokedHero()
    {
        //Get a random hero
        Random rnd = new Random();

        List<HeroCard> heros = HeroZone.Where(c => c is HeroCard).Cast<HeroCard>().ToList();

        return heros[rnd.Next(heros.Count)];
    }

    ///<summary>
    ///Attacks directly to the enemy
    ///</summary>
    ///<param name="hero">Hero that will attack</param>
    ///<param name="enemy">Enemy player</param>
    private void DirectAttack(HeroCard hero, AIPlayer enemy)
    {
        enemy.UpdateLife(-hero.Attack);
    }

    ///<summary>
    ///Checks if there is a hero on the field
    ///</summary>
    ///<returns>True if there is a hero on the field, false otherwise</returns>
    public bool HasHeroOnField()
    {
        return HeroZone.Count > 0;
    }

    ///<summary>
    ///Checks if there is a hero on the hand
    ///</summary>
    ///<returns>True if there is a hero on the hand, false otherwise</returns>
    private bool HasHeroOnHand()
    {
        return Find(Hand, x => x is HeroCard);
    }

    ///<summary>
    ///Checks if there is an item on the hand
    ///</summary>
    ///<returns>True if there is an item on the hand, false otherwise</returns>
    private bool HasItemOnHand()
    {
        return Find(Hand, x => x is ItemCard);
    }

    ///<summary>
    ///Checks if any element in the collection satisfies the condition
    ///</summary>
    ///<param name="collection">Collection to check</param>
    ///<param name="match">Condition to check</param>
    ///<returns>True if any element in the collection satisfies the condition, false otherwise</returns>
    public bool Find(List<SimpleCard> collection, Predicate<SimpleCard> match)
    {
        return collection.Exists(match);
    }

    ///<summary>
    /// Checks if  a hero can be invoked
    ///</summary>
    ///<returns>True if a hero can be invoked, false otherwise</returns>
    public bool CanInvokeHero()
    {
        return (HasHeroOnHand() && PlayerField.CanInvokeHero());
    }

    ///<summary>
    /// Checks if an item can be equipped
    ///</summary>
    ///<returns>True if an item can be equipped, false otherwise</returns>
    public bool CanEquipItem()
    {
        return (HasItemOnHand() && HasHeroOnField() && PlayerField.CanEquipItem());
    }

    ///<summmary>
    /// Gets the hero with the higher attack given a certain collection
    ///</summary>
    ///<param name="collection">Collection to select hero from</param>
    ///<returns>Hero with the higher attack from the collection</returns>
    private static HeroCard GetHeroWithHigherAttack(IEnumerable<ICard> collection)
    {
        //Filter the hand to get only the heroes
        IEnumerable<HeroCard> heros = collection.OfType<HeroCard>();

        return heros.MaxBy(x => x.Attack)!;
    }

    ///<summary>
    ///Gets the hero with the lowest defense given a certain collection
    ///</summary>
    ///<param name="collection">Collection to select hero from</param>
    ///<returns>Hero with the lowest defense from the collection</returns>
    private static HeroCard GetHeroWithLowestDefense(IEnumerable<ICard> collection)
    {
        //Filter the enemy field to get only the heroes
        IEnumerable<HeroCard> heros = collection.OfType<HeroCard>();

        return heros.MinBy(x => x.Defense)!;
    }

}