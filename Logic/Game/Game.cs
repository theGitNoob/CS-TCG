using Cards;
using Deck;
using Player;
using Habilitie;

using System.Text.Json;

namespace Game;

/// <summary>
/// The bridge between UI and Logic
/// </summary>
public static class GameController
{
    /// <summary>
    /// The cards in the game
    /// </summary>
    public static List<SimpleCard> Cards { get; private set; } = new List<SimpleCard>();

    /// <summary>
    /// The path to the items.json file
    /// </summary>
    static string itemsFilePath = "../Content/items.json";

    /// <summary>
    /// The path to the heros.json file
    /// </summary>
    static string heroesFilePath = "../Content/heroes.json";

    /// <summary>
    /// The path to the cards directory
    /// </summary>
    static string cardsDir = "../Content/";

    /// <summary>
    /// The Initial HP points of each player
    /// </summary>
    static int _hpPoints { get; set; } = 4000;

    /// <summary>
    /// The Initial Cards of each player
    /// </summary>
    static int _initialCards { get; set; } = 5;

    /// <summary>
    /// The number of cards each player can draw per turn
    /// </summary>
    static int _cardsPerTurn { get; set; } = 1;

    /// <summary>
    /// The minimum number of cards a valid Deck should have
    /// </summary>
    static int _minDeckCards { get; set; } = 1;

    /// <summary>
    /// The maximum number of cards a valid Deck should have
    /// </summary>
    static int _maxDeckCards { get; set; } = 1;

    /// <summary>
    /// The number of players in the game
    /// </summary>
    static int _playersCnt { get; set; } = 2;


    /// <summary>
    /// Starts the game
    /// </summary>
    public static void StartGame()
    {
        LoadCards();
    }


    /// <summary>
    /// Creates a new `Effect` if is correct
    /// </summary>
    /// <param name="condition">The condition for the effect</param>
    /// <param name="action">The action for the effect</param>
    /// <returns>A new `Effect`</returns>
    static Effect CreateEffect(string condition, string action)
    {
        Effect.CheckIsCorrect<SimplePlayer>(condition, action, "Player");

        Effect effect = new Effect(condition, action, "Player");

        return effect;
    }

    /// <summary>
    /// Creates a new `HeroCard`
    /// </summary>
    /// <param name="name">The name of the `HeroCard`</param>
    /// <param name="attack">The attack of the `HeroCard`</param>
    /// <param name="defense">The defense of the `HeroCard`</param>
    /// <param name="description">The description of the card</param>
    /// <param name="condition">The condition for the effect</param>
    /// <param name="action">The action for the effect</param>
    /// <exception cref="ArgumentNullException">Thrown when the name, description, condition or action is null</exception>
    /// <exception cref="Exception">Thrown when there is already another card with the same name</exception>
    /// <exception cref="CompilationErrorException">Thrown when the condition or action are not correct</exception>
    public static void CreateHeroCard(string? name, int attack, int defense, string? description, string? condition, string? action)
    {
        if (name == null) throw new ArgumentNullException(nameof(name));
        if (description == null) throw new ArgumentNullException(nameof(description));
        if (condition == null) throw new ArgumentNullException(nameof(condition));
        if (action == null) throw new ArgumentNullException(nameof(action));

        Effect effect = CreateEffect(condition, action);

        HeroCard card = new HeroCard(name, attack, defense, description, effect);

        if (IsUnique(card))
        {
            Cards.Add(card);

            SaveCards();
        }

        else
        {
            throw new Exception($"There is alrady another card with the specified name {name}");
        }
    }


    /// <summary>
    /// Checks if there exists another card with the same name
    /// </summary>
    /// <param name="newCard">The card to check if already exists</param>
    /// <returns>True if the card doensn't currently exists, false otherwise</returns>
    static bool IsUnique(SimpleCard newCard)
    {
        return !Cards.Exists(card => card.Name == newCard.Name);

    }

    /// <summary>
    /// Creates a new `ItemCard`
    /// </summary>
    /// <param name="name">The name of the `ItemCard`</param>
    /// <param name="description">The description of the card</param>
    /// <param name="condition">The condition for the effect</param>
    /// <param name="action">The action for the effect</param>
    /// <exception cref="ArgumentNullException">Thrown when the name, description, condition or action is null</exception>
    /// <exception cref="Exception">Thrown when there is already another card with the same name</exception>
    public static void CreateItemCard(string? name, string? description, string? condition, string? action)
    {
        if (name == null) throw new ArgumentNullException(nameof(name));
        if (description == null) throw new ArgumentNullException(nameof(description));
        if (condition == null) throw new ArgumentNullException(nameof(condition));
        if (action == null) throw new ArgumentNullException(nameof(action));


        Effect effect = CreateEffect(condition, action);

        ItemCard card = new ItemCard(name, description, effect);

        if (IsUnique(card))
        {
            Cards.Add(card);

            SaveCards();
        }

        else
        {
            throw new Exception($"There is alrady another card with the specified name {name}");
        }
    }


    /// <summary>
    /// Deserializes a `List`
    /// from a string formatted as json
    /// </summary>
    /// <param name = "jsonFile"> The string to deserialize</param>
    /// <returns>A string formatted as json</returns>
    static List<ItemCard> DeserializeItem(string jsonFile)
    {
        List<ItemCard> items = JsonSerializer.Deserialize<List<ItemCard>>(jsonFile)!;
        return items;
    }

    /// <summary>
    /// Deserializes a `List<HeroCard>` from a string formated as json
    /// </summary>
    /// <param name = "jsonFile"> The string to deserialize</param>
    /// <returns>A string formated as json</returns>
    static List<HeroCard> DeserializeHeros(string jsonFile)
    {
        List<HeroCard> heroes = JsonSerializer.Deserialize<List<HeroCard>>(jsonFile)!;
        return heroes;
    }


    /// <summary>
    /// Serializes a `List<ItemCard>` into a string formatted as json
    /// </summary>
    /// <returns>A string formatted as json</returns>
    static string SerializeHeroes()
    {
        List<HeroCard> heroes = Cards.OfType<HeroCard>().ToList();

        if (heroes.Count == 0) return "";

        string jsonFile = JsonSerializer.Serialize<List<HeroCard>>(heroes);

        return jsonFile;
    }

    /// <summary>
    /// Serializes a `List<ItemCard>` into a string formatted as json
    /// </summary>
    /// <returns>A string formatted as json</returns>
    static string SerializeItems()
    {
        List<ItemCard> items = Cards.OfType<ItemCard>().ToList();

        if (items.Count == 0) return "";

        string jsonFile = JsonSerializer.Serialize<List<ItemCard>>(items);

        return jsonFile;
    }


    /// <summary>
    /// Loads cards from disk on json format
    /// </summary>
    static void LoadCards()
    {
        if (!Directory.Exists(cardsDir))
        {
            Directory.CreateDirectory(cardsDir);
        }

        if (!File.Exists(itemsFilePath))
        {
            File.Create(itemsFilePath).Dispose();
        }

        if (!File.Exists(heroesFilePath))
        {
            File.Create(heroesFilePath).Dispose();
        }

        string heroesFile = File.ReadAllText(heroesFilePath);

        if (heroesFile != "")
        {
            var aux = DeserializeHeros(heroesFile);
            Cards.AddRange(aux);
        }

        string itemsFile = File.ReadAllText(itemsFilePath);

        if (itemsFile != "")
        {
            var aux = DeserializeItem(itemsFile);
            Cards.AddRange(aux);
        }

    }

    /// <summary>
    /// Saves cards to disk on json format
    /// </summary>
    static void SaveCards()
    {
        string itemsJsonFile = SerializeItems();

        File.WriteAllText(itemsFilePath, itemsJsonFile);

        string heroesJsonFile = SerializeHeroes();

        File.WriteAllText(heroesFilePath, heroesJsonFile);
    }


    /// <summary>
    /// Creates a new game
    /// </summary>
    public static void NewGame(SimplePlayer p1, SimplePlayer p2)
    {
        if (p1 == null) throw new ArgumentNullException($"{nameof(p1)} can't be null");
        if (p2 == null) throw new ArgumentNullException($"{nameof(p2)} can't be null");


        GameLoop.StartGame(_initialCards, _cardsPerTurn, p1, p2);
    }

    public static void ChangeDefaults(int hpPoints, int initialCards, int cardsPerTurn, int minDeckCards, int maxDeckCards, int playersCnt)
    {
        if (hpPoints < 1) throw new ArgumentOutOfRangeException(nameof(hpPoints), "The hp points must be greater than 0");
        if (initialCards < 1) throw new ArgumentOutOfRangeException(nameof(initialCards), "The initial cards must be greater than 0");
        if (cardsPerTurn < 1) throw new ArgumentOutOfRangeException(nameof(cardsPerTurn), "The cards per turn must be greater than 0");
        if (minDeckCards < 1) throw new ArgumentOutOfRangeException(nameof(minDeckCards), "The minimum deck cards must be greater than 0");
        if (maxDeckCards < 1) throw new ArgumentOutOfRangeException(nameof(maxDeckCards), "The maximum deck cards must be greater than 0");
        if (minDeckCards > maxDeckCards) throw new ArgumentException($"{nameof(minDeckCards)} should be less or equal than {nameof(maxDeckCards)}");
        if (playersCnt < 2) throw new ArgumentOutOfRangeException(nameof(playersCnt), "The players count must be greater than 1");

        _hpPoints = hpPoints;
        _initialCards = initialCards;
        _cardsPerTurn = cardsPerTurn;
        _minDeckCards = minDeckCards;
        _maxDeckCards = maxDeckCards;
        _playersCnt = playersCnt;

    }

    /// <summary>
    /// Saves all the cards to disk and gracefully shutdown
    /// </summary>
    public static void ExitGame()
    {
        Environment.Exit(0);
    }
}