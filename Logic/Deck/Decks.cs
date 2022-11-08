using Cards;
namespace Deck;
public class Decks
{
    public List<Card> Deck{get;set;}
    public void RemoveCard(int numberCards){
        for (int i = 0; i < numberCards; i++)
        {
            Deck.RemoveAt(i);
        }
    }
}
