using Cards;
using Deck;
using Field;

namespace Player;

/// <summary>
/// Represents a player in the game
/// </summary>
public interface IPlayer
{
    /// <summary>
    /// Player's Name
    /// </summary>
    string Name { get; }

    /// <summary>
    /// The player's health points
    /// </summary>
    int Hp { set; get; }

    /// <summary>
    /// The player's deck
    /// </summary>
    SimpleDeck Deck { get; }

    /// <summary>
    /// The player's hand
    /// </summary>
    List<SimpleCard> Hand { get; }

    /// <summary>
    /// The player's field
    /// </summary>
    SimpleField PlayerField { get; }

    /// <summary>
    /// The player's hero zone
    /// </summary>
    List<HeroCard> HeroZone { get; }

    /// <summary>
    /// The player's item zone
    /// </summary>
    List<ItemCard> ItemZone { get; }

    void Play(bool canAttack);
}