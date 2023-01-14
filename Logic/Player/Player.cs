namespace Player;

using Cards;
using Field;
using Deck;

public interface IPlayer
{
    int HP { set; get; }

    SimpleDeck Deck { get; }

    List<ICard> Hand { get; }

    SimpleField PlayerField { get; }

    void DrawCards(int cnt);

    void InvokeHero(HeroCard hero);

    void EquipItem(HeroCard hero, ItemCard item);

    void AttackHero(HeroCard hero, HeroCard target, IPlayer enemy);

    void EndTurn();

    void UpdateLife(int value);

}

public class HumanPlayer : IPlayer
{
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
    public List<ICard> Hand { get; private set; }

    public SimpleDeck Deck { get; private set; }

    public SimpleField PlayerField { get; private set; }


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
    ///Invoke a given hero to the field
    ///</summary>
    ///<param name="hero">Hero to be invoked</param>
    public void InvokeHero(HeroCard hero)
    {
        throw new NotImplementedException();
    }

    public void EquipItem(HeroCard hero, ItemCard item)
    {
        throw new NotImplementedException();
    }


    ///<summary>
    ///Attack a given hero with another hero
    ///</summary>
    ///<param name="hero">Hero that is attacking</param>
    ///<param name="target">Hero that is being attacked</param>
    ///<param name="enemy">Enemy player</param>

    public void AttackHero(HeroCard hero, HeroCard target, IPlayer enemy)
    {
        int diff = hero.Attack - target.Defense;
        if (diff < 0)
        {
            UpdateLife(diff);
            PlayerField.RemoveHero(target);
        }
        else
        {
            enemy.UpdateLife(diff);
            enemy.PlayerField.RemoveHero(target);
        }

    }

    public void EndTurn()
    {
        throw new NotImplementedException();
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
    ///<param name="hp">Initial life of the player</param>
    ///<param name="maxHeroCards">Max number of Hero cards on the Field</param>
    ///<param name="maxItemCards">Max number of Item cards on the Field</param>
    ///<param name="deck">Deck of the player</param>
    ///<param name="field">Field of the player</param>
    ///<returns>A new Human Player</returns>

    public HumanPlayer(int hp, int maxHeroCards, int maxItemCards, SimpleDeck deck)
    {
        HP = hp;
        Deck = deck;
        Hand = new List<ICard>();
        PlayerField = new SimpleField(maxHeroCards, maxItemCards);
    }
}
