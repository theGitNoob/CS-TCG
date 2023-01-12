namespace Deck
{

    public interface IDeck
    {

        //Holds the Collection of cards
        IEnumerable<ICard> Cards { get; set; }

        //Retrieves a finite number of cards from the begining 
        IEnumerable<ICard> GetTop(int numberOfCards);

        //Retrieves a finite number of cards from the end
        IEnumerable<ICard> GetBottom(int numberOfCards);


        //Holds the number of cards on the Deck Currently
        int CardsLeft { get; }

        //Maximum posible number of cards of the deck
        int MaxCards { get; set; }

        //Minimun posible numbers of cards of the deck
        int MinCards { get; set; }

        //Place a finite number of cards at the bottom of the deck
        void PlaceBottom(IEnumerable<ICard> cards);

        //Place a finite number of cards at the top of the deck
        void PlaceTop(IEnumerable<ICard> cards);

        //Shuffles the deck
        void Shuffle();

        //Checks wether no card lefts on deck
        bool IsEmpty();

        //Gets the Card at given position
        ICard GetIthCard(int cardIdx);

    }

    public class SimpleDeck : IDeck
    {

        public int MaxCards { set; get; }

        public int MinCards { set; get; }

        public IEnumerable<ICard> Cards { set; get; }

        public int CardsLeft { get => Cards.Count(); }

        public SimpleDeck(IEnumerable<ICard> cards, int minCards, int maxCards)
        {
            Cards = cards;
            MinCards = minCards;
            MaxCards = maxCards;

        }

        public bool IsEmpty()
        {
            return CardsLeft == 0;
        }

        public void PlaceBottom(IEnumerable<ICard> cards)
        {
            Cards = Cards.Concat(cards);
        }

        public void PlaceTop(IEnumerable<ICard> cards)
        {
            Cards = cards.Concat(Cards);
        }



        //Swaps two elements from the deck
        public void Swap(int i, int j)
        {
            IList<ICard> aux = Cards.ToList();
            ICard temp = aux[i];
            aux[i] = aux[j];
            aux[j] = temp;
            Cards = aux;

        }

        //Shuffles a collection of elements using
        //Fisher-Yates-Durstenfeld shuffle
        public void Shuffle()
        {
            Random rand = new Random();

            IList<ICard> aux = Cards.ToList();

            for (int i = CardsLeft - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1);

                Swap(i, j);
            }

            Cards = aux;
        }
        public IEnumerable<ICard> GetTop(int numberOfCards)
        {
            if (numberOfCards <= 0 || numberOfCards > CardsLeft)
            {
                throw new ArgumentException("Number of cards should be greater than 0, and equal or less than de `CardsLeft`");
            }

            return Cards.Take(numberOfCards);

        }


        public IEnumerable<ICard> GetBottom(int numberOfCards)
        {
            if (numberOfCards <= 0 || numberOfCards > CardsLeft)
            {
                throw new ArgumentException("Number of cards should be greater than 0, and equal or less than de `CardsLeft`");
            }

            return Cards.TakeLast(numberOfCards);
        }

        public ICard GetIthCard(int cardIdx)
        {
            if (cardIdx < 0 || cardIdx >= CardsLeft)
            {
                throw new ArgumentException("Card index must be between 0 and `CardsLeft`- 1");
            }

            return Cards.ElementAt(cardIdx);

        }


        //Retrieves a random card from the Deck
        public ICard GetRandomCard()
        {
            Random rand = new Random();

            int cardIdx = rand.Next(0, CardsLeft);

            return GetIthCard(cardIdx);

        }
    }

}
