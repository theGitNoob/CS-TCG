using Game.Cards;

namespace Deck;

/// <summary>
/// Represents a deck of cards
/// </summary>
public interface IDeck
{
    /// <summary>
    /// The cards in the deck
    /// </summary>
    List<SimpleCard> Cards { get; }

    /// <summary>
    /// Gets the n top cards of the deck and removes them from the deck
    /// </summary>
    /// <param name="numberOfCards">Number of cards to be taken from the top of the deck</param>
    /// <returns>A list of the top n cards of the deck</returns>
    List<SimpleCard> GetTop(int numberOfCards);

    /// <summary>
    /// Gets the n bottom cards of the deck and removes them from the deck
    /// </summary>
    /// <param name="numberOfCards">Number of cards to be taken from the bottom of the deck</param>
    /// <returns>A list of the bottom n cards of the deck</returns>
    List<SimpleCard> GetBottom(int numberOfCards);

    /// <summary>
    /// Gets the number of cards in the deck
    /// </summary>
    int CardsLeft { get; }

    /// <summary>
    /// Places a list of cards at the bottom of the deck
    /// </summary>
    /// <param name="cards">List of cards to be placed at the bottom of the deck</param>
    void PlaceBottom(List<SimpleCard> cards);

    /// <summary>
    /// Places a list of cards at the top of the deck
    /// </summary>
    /// <param name="cards">List of cards to be placed at the top of the deck</param>
    void PlaceTop(List<SimpleCard> cards);

    /// <summary>
    /// Shuffles all the Deck
    /// </summary>
    void Shuffle();

    /// <summary>
    /// Checks if the deck is empty
    /// </summary>
    bool IsEmpty();


    /// <summary>
    ///Gets the card at the given index
    /// </summary>
    SimpleCard GetIthCard(int cardIdx);

}