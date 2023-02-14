namespace Cards;

/// <summary>
/// Represents a card
/// </summary>
public class SimpleCard : ICard
{
    /// <summary>
    /// The type of the card
    /// </summary>
    public CardType Type { get; protected init; }

    /// <summary>
    /// The id of the card
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The name of the card
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The description of the card
    /// </summary>
    public string Description { get; init; }

    /// <summary>
    /// The effect of the card
    /// </summary>
    public Effect.Effect Effect { get; init; }

    /// <summary>
    /// Creates a new card
    /// </summary>
    /// <param name="name">The name of the card</param>
    /// <param name="description">The description of the card</param>
    /// <param name="effect">The effect of the card</param>
    /// <exception cref="ArgumentNullException">Thrown when the name, description or effect is null</exception>
    /// <returns>A new card</returns>
    public SimpleCard(string name, string description, Effect.Effect effect)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));

        Description = description ?? throw new ArgumentNullException(nameof(description));

        Effect = effect ?? throw new ArgumentNullException(nameof(effect));

        Id = GenRandId();
    }

    /// <summary>
    /// Generates a random id
    /// </summary>
    /// <returns>A random id</returns>
    public int GenRandId() => Guid.NewGuid().ToString().GetHashCode();
}