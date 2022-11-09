using Cards;
using Deck;
namespace Field;
public class Field
{
    public List<Tuple<int,Card>> MonsterZone{get;set;}
    public List<Card> Graveyard{get;set;}
    public Decks Deck{get;set;}
    public Decks ExtraDeck{get;set;}
}
