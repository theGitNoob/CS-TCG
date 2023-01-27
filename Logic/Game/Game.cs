using Cards;
using Field;
using Deck;
using Player;
using Effect;

using System.Text.Json;
namespace Game;
public static class GameController
{

    ///<summary>
    ///The cards in the game
    ///</summary>
    public static List<SimpleCard> Cards { get; private set; } = new List<SimpleCard>();

    ///<summary>
    ///The path to the cards.json file
    ///</summary>
    public static string cardsPath = "../Content/cards.json";

    ///<summary>
    ///Starts the game
    ///</summary>
    public static void StartGame()
    {

        LoadCards();

        Cards.ForEach(card =>
        {
            System.Console.WriteLine(card.Id);
            System.Console.WriteLine(card.Effect);
            Habilitie x = DeserializeEffect(card.Effect);
            System.Console.WriteLine(x.ActionString);
            System.Console.WriteLine(x.ConditionString);
        });
    }

    ///<summary>
    ///Creates a new `HeroCard`
    ///</summary>
    ///<param name="name">The name of the card</param>
    ///<param name="attack">The attack of the `HeroCard`</param>
    ///<param name="defense">The defense of the `HeroCard`</param>
    ///<param name="description">The description of the card</param>
    ///<param name="condition">The condition for the effect</param>
    ///<param name="action">The action for the effect</param>
    ///<exception cref="ArgumentNullException">Thrown when the name, description, condition or action is null</exception>
    public static void CreateHeroCard(string name, int attack, int defense, string description, string condition, string action)
    {
        if (name == null) throw new ArgumentNullException(nameof(name));
        if (description == null) throw new ArgumentNullException(nameof(description));
        if (condition == null) throw new ArgumentNullException(nameof(condition));
        if (action == null) throw new ArgumentNullException(nameof(action));

        Habilitie effect = new Habilitie(condition, action);

        string jsonEffect = SerializeEffect(effect);

        HeroCard card = new HeroCard(name, attack, defense, description, jsonEffect);

        Cards.Add(card);
        SaveCards();
    }


    public static void CreateItemCard(string name, string description, string condition, string action)
    {
        if (name == null || description == null || condition == null || action == null) throw new ArgumentNullException();

        Habilitie effect = new Habilitie(condition, action);

        string jsonEffect = SerializeEffect(effect);

        ItemCard card = new ItemCard(name, description, jsonEffect);

        Cards.Add(card);
        SaveCards();
    }


    ///<summary>
    ///Deserializes a string formated as json into a `List<SimpleCards>`
    ///</summary>
    ///<param name="jsonFile">The json file to deserialize</param>
    ///<returns>A `List<SimpleCards>` with all the cards</returns>
    public static List<SimpleCard> DeserializeCards(string jsonFile)
    {
        List<SimpleCard> cards = JsonSerializer.Deserialize<List<SimpleCard>>(jsonFile)!;
        return cards;
    }

    ///<summary>
    ///Serializes a `List<SimpleCards>` into a string formated as json
    ///</summary>
    ///<returns>A string formated as json</returns>
    public static string SerializeCards()
    {
        if (Cards.Count == 0) return "";

        string jsonFile = JsonSerializer.Serialize<List<SimpleCard>>(Cards);
        return jsonFile;
    }

    ///<summary>
    ///Loads cards from disk on json format
    ///</summary>
    public static void LoadCards()
    {
        string jsonFile = File.ReadAllText(cardsPath);

        if (jsonFile == "") return;

        Cards = DeserializeCards(jsonFile);
    }

    ///<summary>
    ///Saves cards to disk on json format
    ///</summary>
    public static void SaveCards()
    {
        string jsonFile = SerializeCards();
        File.WriteAllText(cardsPath, jsonFile);
    }


    ///<summary>
    ///Creates a new game
    ///</summary>
    ///<param name="maxHeroCards">The maximum number of hero cards a player can have</param>
    ///<param name="maxItemCards">The maximum number of item cards a player can have</param>
    ///<param name="minDeckCards">The minimum number of cards a player can have in their deck</param>
    ///<param name="maxDeckCards">The maximum number of cards a player can have in their deck</param>
    public static void NewGame( AIPlayer p1, AIPlayer p2, int maxHeroCards = 5, int maxItemCards = 5, int minDeckCards = 1, int maxDeckCards = 50)
    {
        SimpleField field = new SimpleField(maxHeroCards, maxItemCards);
        SimpleDeck deck = new SimpleDeck(Cards, minDeckCards, maxDeckCards);
        p1 = new AIPlayer(4000, maxHeroCards, maxItemCards, deck);
        p2 = new AIPlayer(4000, maxHeroCards, maxItemCards, deck);

    }


    ///<summary>
    ///Serializes a `Habilitie` to json format
    ///</summary>
    ///<param name="effect">The `Habilitie` to serialize</param>
    ///<exception cref="ArgumentNullException">Thrown when the `Habilitie` is null</exception>
    ///<returns>A string formated as json</returns>
    static string SerializeEffect(Habilitie effect)
    {
        if (effect == null) throw new ArgumentNullException();

        string jsonEffect = JsonSerializer.Serialize<Habilitie>(effect);

        return jsonEffect;
    }

    ///<summary>
    ///Deserializes a string formated as json into a `Habilitie`
    ///</summary>
    ///<param name="jsonEffect">The string to deserialize</param>
    ///<exception cref="ArgumentNullException">Thrown when the string is null</exception>
    ///<returns>A `Habilitie`</returns>
    static Habilitie DeserializeEffect(string jsonEffect)
    {
        if (jsonEffect == "") throw new ArgumentNullException();

        Habilitie effect = JsonSerializer.Deserialize<Habilitie>(jsonEffect)!;

        return effect;
    }

    public static void ExitGame()
    {
        Environment.Exit(0);
    }

}
