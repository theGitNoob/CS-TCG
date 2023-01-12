using Cards;
using System.Text.Json;
namespace Game;
public static class Game
{

    //Holds all the cards
    public static List<SimpleCard> Cards = new List<SimpleCard>();

    //Path to cards in json format
    public static string cardsPath = "../Content/cards.json";

    //Initializes the game
    public static void StartGame()
    {
        LoadCards();
        Cards.ForEach(card => System.Console.WriteLine(card.Id));
    }

    //Creates a new card and add it to `List<Icard>`
    public static void CreateCard(string name, string description, string effect, CardType type)
    {
        SimpleCard card = new SimpleCard(name, description, effect);

        switch (type)
        {
            case CardType.Hero:
                {
                    card = new HeroCard(name, description, effect);
                    break;
                }
            case CardType.Item:
                {
                    card = new ItemCard(name, description, effect);
                    break;
                }
        }

        Cards.Add(card);
    }


    //Deserializes Cards in json format onto a `List<Icard>`
    public static List<SimpleCard> DeserializeCards(string jsonFile)
    {
        List<SimpleCard> cards = JsonSerializer.Deserialize<List<SimpleCard>>(jsonFile)!;
        return cards;
    }

    //Serializes the `List<ICard>` onto a string formated as json
    public static string SerializeCards()
    {
        if (Cards.Count == 0) return "";

        string jsonFile = JsonSerializer.Serialize<List<SimpleCard>>(Cards);
        return jsonFile;
    }

    //Load cards saved in json format
    public static void LoadCards()
    {
        string jsonFile = File.ReadAllText(cardsPath);

        if (jsonFile == "") return;

        Cards = DeserializeCards(jsonFile);
    }

    //Save cards to disk on json format
    public static void SaveCards()
    {
        string jsonFile = SerializeCards();
        File.WriteAllText(cardsPath, jsonFile);
    }

}
