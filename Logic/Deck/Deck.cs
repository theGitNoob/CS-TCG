﻿namespace Deck;
using Cards;

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
    /// Gets the maximum number of cards in the deck
    /// </summary>
    int MaxCards { get; }

    /// <summary>
    /// Gets the minimum number of cards in the deck
    /// </summary>
    int MinCards { get; }

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

/// <summary>
/// Represents a deck of cards
/// </summary>
public class SimpleDeck : IDeck
{

    public int MaxCards { private set; get; }

    public int MinCards { private set; get; }

    public List<SimpleCard> Cards { private set; get; }

    public int CardsLeft { get => Cards.Count(); }


    /// <summary>
    /// Creates a new deck with a given number of cards
    /// </summary>
    /// <param name="cards">List of cards to be added to the deck</param>
    /// <param name="minCards">Minimum number of cards in the deck</param>
    /// <param name="maxCards">Maximum number of cards in the deck</param>
    public SimpleDeck(List<SimpleCard> cards, int minCards, int maxCards)
    {
        Cards = new List<SimpleCard>();

        foreach (SimpleCard card in cards)
        {
            Cards.Add(card);
        }

        MinCards = minCards;
        MaxCards = maxCards;

    }


    public bool IsEmpty()
    {
        return CardsLeft == 0;
    }

    /// <summary>
    /// Places a list of cards at the bottom of the deck
    /// </summary>
    /// <param name="cards">List of cards to be placed at the bottom of the deck</param>
    public void PlaceBottom(List<SimpleCard> cards)
    {
        Cards = Cards.Concat(cards).ToList();
    }

    /// <summary>
    /// Places a list of cards at the top of the deck
    /// </summary>
    /// <param name="cards">List of cards to be placed at the top of the deck</param>
    public void PlaceTop(List<SimpleCard> cards)
    {
        Cards = cards.Concat(Cards).ToList();
    }



    /// <summary>
    /// Swaps two cards in the deck
    /// </summary>
    /// <param name="i">Index of the first card</param>
    /// <param name="j">Index of the second card</param>
    public void Swap(int i, int j)
    {
        SimpleCard temp = Cards[i];

        Cards[i] = Cards[j];
        Cards[j] = temp;
    }

    /// <summary>
    ///Shuffles a collection of elements using
    ///Fisher-Yates-Durstenfeld shuffle
    /// </summary>
    public void Shuffle()
    {
        Random rand = new Random();

        for (int i = CardsLeft - 1; i > 0; i--)
        {
            int j = rand.Next(i + 1);

            Swap(i, j);
        }

    }


    /// <summary>
    /// Gets the n top cards of the deck and removes them from the deck
    /// </summary>
    /// <param name="numberOfCards">Number of cards to be taken from the top of the deck</param>
    /// <exception cref="ArgumentException">
    /// Thrown when the number of cards is less than 0 or greater than the number of cards left in the deck
    /// </exception>
    /// <returns>A list of the top n cards of the deck</returns>
    public List<SimpleCard> GetTop(int numberOfCards)
    {
        if (numberOfCards <= 0 || numberOfCards > CardsLeft)
            throw new ArgumentException("Number of cards should be greater than 0, and equal or less than de `CardsLeft`");

        List<SimpleCard> aux = Cards.Take(numberOfCards).ToList();

        Cards.RemoveRange(0, numberOfCards);

        return aux;

    }

    /// <summary>
    /// Gets the n bottom cards of the deck and removes them from the deck
    /// </summary>
    /// <param name="numberOfCards">Number of cards to be taken from the bottom of the deck</param>
    /// <returns>A list of the bottom n cards of the deck</returns>
    public List<SimpleCard> GetBottom(int numberOfCards)
    {
        if (numberOfCards <= 0 || numberOfCards > CardsLeft)
        {
            throw new ArgumentException("Number of cards should be greater than 0, and equal or less than de `CardsLeft`");
        }

        List<SimpleCard> aux = Cards.TakeLast(numberOfCards).ToList();

        Cards.RemoveRange(CardsLeft - numberOfCards, numberOfCards);

        return aux;
    }

    /// <summary>
    /// Gets the ith card of the deck
    /// </summary>
    /// <param name="cardIdx">Index of the card to be taken</param>
    /// <returns>The ith card of the deck</returns>
    public SimpleCard GetIthCard(int cardIdx)
    {
        if (cardIdx < 0 || cardIdx >= CardsLeft)
        {
            throw new ArgumentException("Card index must be between 0 and `CardsLeft`- 1");
        }

        return Cards.ElementAt(cardIdx);
    }


    /// <summary>
    /// Returns a random card from the deck
    /// </summary>
    public SimpleCard GetRandomCard()
    {
        Random rand = new Random();

        int cardIdx = rand.Next(0, CardsLeft);

        return GetIthCard(cardIdx);
    }
}


