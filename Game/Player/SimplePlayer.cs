namespace Player;

using Cards;
using Field;
using Deck;

/// <summary>
/// Represents a player in the game
/// </summary>
public class SimplePlayer : IPlayer
{
    public string Name { get; set; }

    /// <summary>
    /// The player's health points
    /// </summary>
    private int _hp;

    public int Hp
    {
        get => _hp;
        set => _hp = value < 0 ? 0 : value;
    }

    public List<SimpleCard> Hand { get; protected set; }

    public SimpleDeck Deck { get; protected set; }

    public SimpleField PlayerField { get; protected set; }

    public List<HeroCard> HeroZone => PlayerField.HeroZone;

    public List<ItemCard> ItemZone => PlayerField.ItemZone;

    public List<SimpleCard> CementeryZone => PlayerField.CementeryZone;

    public SimplePlayer? Enemy { get; private set; }

    /// <summary>
    /// Draw a given number of cards from the deck
    /// </summary>
    /// <param name="cnt">Number of cards to be drawn</param>
    public void DrawCards(int cnt)
    {
        List<SimpleCard> cards = Deck.GetTop(cnt);
        Hand.AddRange(cards);
    }

    /// <summary>
    /// Invoke a given hero to the field and removes it from the hand
    /// </summary>
    /// <param name="hero">Hero to be invoked</param>
    public void InvokeHero(HeroCard hero)
    {
        PlayerField.PlaceHero(hero);
        Hand.Remove(hero);
    }

    /// <summary>
    /// Equip a given item to a given hero, places it on the field and removes it from hand
    /// </summary>
    /// <param name="hero">Hero that will equip the item</param>
    /// <param name="item">Item to be equipped</param>
    public void EquipItem(HeroCard hero, ItemCard item)
    {
        hero.EquipItem(item);
        PlayerField.PlaceItem(item);
        item.EquipToHero(hero);
        Hand.Remove(item);
    }

    /// <summary>
    /// Attack a given hero with another given hero
    /// </summary>
    /// <param name="hero">Hero that will attack</param>
    /// <param name="target">Hero that will be attacked</param>
    public void AttackHero(HeroCard hero, HeroCard target)
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
            Enemy?.PlayerField.RemoveHero(target);
        }
        else
        {
            Enemy?.UpdateLife(-diff);
            Enemy?.PlayerField.RemoveHero(target);
        }
    }


    /// <summary>
    /// Update the life of the player
    /// </summary>
    /// <param name="value">Value to be added to the life</param>
    public void UpdateLife(int value)
    {
        Hp = Hp + value;
    }

    /// <summary>
    ///  Plays according to user input
    /// </summary>
    /// <param name="canAttack"></param>
    /// <exception cref="NullReferenceException">Thrown when `Play` is invoked without having set the Enemy</exception>
    public virtual void Play(bool canAttack)
    {
        if (Enemy == null)
            throw new NullReferenceException("Enemy player must be set before start to play");

        var userInput = ConsoleKey.Z;

        HeroCard? selectedHero = null;
        ItemCard? selectedItem = null;
        SimpleCard? selectedHandCard = null;

        while (userInput != ConsoleKey.E)
        {
            userInput = Console.ReadKey(false).Key;

            switch (userInput)
            {
                case ConsoleKey.A:
                {
                    if (selectedHero != null)
                    {
                        if (!Enemy.HasHeroOnField())
                        {
                            DirectAttack(selectedHero);
                        }
                        else
                        {
                            if (int.TryParse(Console.ReadKey(false).KeyChar.ToString(), out var idx) &&
                                Enemy.PlayerField.IsHeroAt(idx))
                            {
                                var enemyHero = Enemy.PlayerField.GetHeroCard(idx);
                                AttackHero(selectedHero, enemyHero);
                            }
                        }
                    }

                    Printer.Print(this, Enemy);

                    break;
                }

                case ConsoleKey.C:
                {
                    var mod = Console.ReadKey(false).Key;

                    switch (mod)
                    {
                        case ConsoleKey.H:
                        {
                            if (selectedHero is not null)
                            {
                                selectedHero.Effect.Activate(this);
                            }

                            break;
                        }
                        case ConsoleKey.I:
                        {
                            if (selectedItem is not null )
                            {
                                selectedItem.Effect.Activate(this);
                            }

                            break;
                        }
                    }

                    Printer.Print(this, Enemy);

                    break;
                }
                case ConsoleKey.D:
                {
                    var mod = Console.ReadKey(false).Key;

                    switch (mod)
                    {
                        case ConsoleKey.H:
                            selectedHero = null;
                            break;
                        case ConsoleKey.I:
                            selectedItem = null;
                            break;
                    }

                    break;
                }
                case ConsoleKey.H:
                {
                    Printer.PrintHelp();
                    break;
                }

                case ConsoleKey.I:
                {
                    if (selectedHandCard is HeroCard hero && PlayerField.CanInvokeHero())
                    {
                        InvokeHero(hero);
                    }
                    else if (selectedHandCard is ItemCard item && PlayerField.CanEquipItem())
                    {
                        var mod = Console.ReadKey(false).KeyChar.ToString();
                        if (int.TryParse(mod, out var idx) && PlayerField.IsHeroAt(idx))
                            EquipItem(PlayerField.GetHeroCard(idx), item);
                    }

                    Printer.Print(this, Enemy);

                    break;
                }

                case ConsoleKey.S:
                {
                    var mod = Console.ReadKey(false).Key;

                    switch (mod)
                    {
                        case ConsoleKey.H:
                        {
                            if (int.TryParse(Console.ReadKey(false).KeyChar.ToString(), out var idx) &&
                                PlayerField.IsHeroAt(idx))
                            {
                                selectedHero = PlayerField.GetHeroCard(idx);
                            }

                            break;
                        }
                        case ConsoleKey.I:
                        {
                            if (int.TryParse(Console.ReadKey(false).KeyChar.ToString(), out var idx) &&
                                PlayerField.IsItemAt(idx))
                            {
                                selectedItem = PlayerField.GetItemCard(idx);
                            }

                            break;
                        }
                        case ConsoleKey.C:
                        {
                            var number = "";
                            ConsoleKeyInfo pressed;
                            do
                            {
                                pressed = Console.ReadKey(false);

                                if (pressed.Key != ConsoleKey.Enter)
                                    number += pressed.KeyChar;
                            } while (pressed.Key != ConsoleKey.Enter);

                            if (int.TryParse(number, out var idx) && idx >= 0 && idx < Hand.Count)
                                selectedHandCard = Hand[idx];
                            break;
                        }
                    }

                    break;
                }
                case ConsoleKey.V:
                {
                    var mod = Console.ReadKey(false).Key;

                    switch (mod)
                    {
                        case ConsoleKey.C:
                        {
                            if (selectedHandCard != null)
                                Printer.ViewCardInfo(selectedHandCard);
                            break;
                        }
                        case ConsoleKey.H:
                        {
                            if (selectedHero != null)
                                Printer.ViewCardInfo(selectedHero);
                            break;
                        }
                        case ConsoleKey.I:
                        {
                            if (selectedItem != null)
                                Printer.ViewCardInfo(selectedItem);
                            break;
                        }
                        case ConsoleKey.V:
                            Printer.PrintHand(this);
                            break;
                    }
                }
                    break;
            }
        }
    }


    /// <summary>
    /// Creates a new Human Player
    /// </summary>
    /// <param name="name"> Name of the player</param>
    /// <param name="hp">Initial life of the player</param>
    /// <param name="maxHeroCards">Max number of Hero cards on the Field</param>
    /// <param name="maxItemCards">Max number of Item cards on the Field</param>
    /// <param name="deck">Deck of the player</param>
    /// <exception cref="ArgumentNullException">Thrown when name or deck are null</exception>
    /// <exception cref="ArgumentException">Thrown when name is not alphanumeric</exception>
    public SimplePlayer(string name, int hp, int maxHeroCards, int maxItemCards, SimpleDeck deck)
    {
        if (name == "") throw new ArgumentNullException(nameof(name));
        if (!name.All(char.IsLetterOrDigit))
            throw new ArgumentException("Player name must be alphanumeric", nameof(name));

        this.Name = name;
        this.Hp = hp;
        this.Deck = deck ?? throw new ArgumentNullException(nameof(deck));
        this.Hand = new List<SimpleCard>();
        this.PlayerField = new SimpleField(maxHeroCards, maxItemCards);
    }

    /// <summary>
    /// Sets the enemy of the Player
    /// </summary>
    /// <param name="enemy">Enemy of the Player</param>
    /// <exception cref="ArgumentNullException"> Thrown when enemy player is null</exception>
    public void SetEnemy(SimplePlayer enemy)
    {
        if (Enemy is not null)
            throw new Exception("Enemy is already set");
        Enemy = enemy ?? throw new ArgumentNullException($"{nameof(enemy)} can't be null");
    }

    /// <summary>
    /// Attacks directly to the enemy
    /// </summary>
    /// <param name="hero">Hero that will attack</param>
    protected void DirectAttack(HeroCard hero)
    {
        Enemy?.UpdateLife(-hero.Attack);
    }

    /// <summary>
    /// Checks if there is a hero on the field
    /// </summary>
    ///<returns>True if there is a hero on the field, false otherwise</returns>
    public bool HasHeroOnField()
    {
        return HeroZone.Count > 0;
    }

    /// <summary>
    /// Checks if  a hero can be invoked
    /// </summary>
    /// <returns>True if a hero can be invoked, false otherwise</returns>
    protected bool CanInvokeHero()
    {
        return (HasHeroOnHand() && PlayerField.CanInvokeHero());
    }

    /// <summary>
    /// Checks if there is a hero on the hand
    /// </summary>
    /// <returns>True if there is a hero on the hand, false otherwise</returns>
    protected bool HasHeroOnHand()
    {
        return Find(Hand, x => x is HeroCard);
    }

    /// <summary>
    /// Checks if there is an item on the hand
    /// </summary>
    /// <returns>True if there is an item on the hand, false otherwise</returns>
    protected bool HasItemOnHand()
    {
        return Find(Hand, x => x is ItemCard);
    }

    /// <summary>
    /// Checks if any element in the collection satisfies the condition
    /// </summary>
    /// <param name="collection">Collection to check</param>
    /// <param name="match">Condition to check</param>
    /// <returns>True if any element in the collection satisfies the condition, false otherwise</returns>
    public bool Find(List<SimpleCard> collection, Predicate<SimpleCard> match)
    {
        return collection.Exists(match);
    }


    /// <summary>
    /// Checks if an item can be equipped
    /// </summary>
    /// <returns>True if an item can be equipped, false otherwise</returns>
    public bool CanEquipItem()
    {
        return (HasItemOnHand() && HasHeroOnField() && PlayerField.CanEquipItem());
    }
}