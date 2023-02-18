using System.Text.Json.Serialization;

namespace Game.Cards;

/// <summary>
/// Represents a card
/// </summary>
public class SimpleCard : ICard
{
    /// <summary>
    /// The type of the card
    /// </summary>
    public CardType Type { get; set; }

    /// <summary>
    /// The id of the card
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The name of the card
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The description of the card
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// The effect of the card
    /// </summary>
    // [JsonIgnore]
    public Effect.Effect Effect { get; set; }


    /// <summary>
    /// Creates a new card
    /// </summary>
    /// <param name="name">The name of the card</param>
    /// <param name="description">The description of the card</param>
    /// <param name="effectString">The effect of the card</param>
    /// <exception cref="ArgumentNullException">Thrown when the name, description or effect is null</exception>
    /// <returns>A new card</returns>
    [JsonConstructor]
    public SimpleCard(string name, string description, string effectString)
    {
        if (string.IsNullOrEmpty(effectString)) throw new ArgumentNullException(nameof(effectString));

        Name = name ?? throw new ArgumentNullException(nameof(name));

        Description = description ?? throw new ArgumentNullException(nameof(description));

        Effect = new Effect.Effect(effectString);

        Id = GenRandId();
    }


    [JsonConstructor]
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