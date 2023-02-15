using Cards;
using System.Text;

namespace Player;

public static class Printer
{
    static readonly StringBuilder Screen = new StringBuilder();

    static string _helpMenu = "";


    static bool _helpActive;


    static bool _viewInfo;



    public static void Print(SimplePlayer p1, SimplePlayer p2)
    {
        Console.Clear();
        Screen.Clear();

        int tabSeparation = 5;

        int blockWidth = (Console.LargestWindowWidth - tabSeparation * 4) / 5;

        int blockHeight = 5;

        Console.Title = "Game";

        PrintPlayer(blockWidth, tabSeparation, $"❤️  {p2.Hp}  ❤️", p2.Name);
        PrintDeckAndCementery(blockWidth, blockHeight, tabSeparation, p2);
        PrintItems(blockWidth, 3, tabSeparation, p2);
        PrintHeroes(blockWidth, blockHeight, tabSeparation, p2);

        PrintHeroes(blockWidth, blockHeight, tabSeparation, p1);
        PrintItems(blockWidth, 3, tabSeparation, p1);
        PrintDeckAndCementery(blockWidth, blockHeight, tabSeparation, p1, true);
        PrintPlayer(blockWidth, tabSeparation, p1.Name, $"❤️  {p1.Hp}  ❤️");

        Console.Write(Screen);
    }

    static void PrintPlayer(int blockWidth, int tabSeparation, string buff1, string buff2)
    {
        Screen.Append(buff1);

        int usedWidth = blockWidth * 5 + tabSeparation * 4;

        for (int i = buff1.Length; i < usedWidth - buff2.Length; i++)
        {
            Screen.Append(" ");
        }

        Screen.AppendLine(buff2);
    }


    private static void PrintDeckAndCementery(int blockWidth, int blockHeight, int tabSeparation, SimplePlayer player,
        bool invert = false)
    {
        string deck = "Deck";
        string cementery = "☠️Cementery☠️";

        int deckCount = player.Deck.CardsLeft;
        int cementeryCount = player.CementeryZone.Count;


        if (invert)
        {
            deck = "☠️Cementery☠️";
            cementery = "Deck";
            deckCount = player.CementeryZone.Count;
            cementeryCount = player.Deck.CardsLeft;
        }

        for (int row = 1; row <= blockHeight; row++)
        {
            for (int block = 1; block <= 5; block++)
            {
                int nameIdx = 0;

                for (int col = 1; col <= blockWidth; col++)
                {
                    if (block is > 1 and < 5)
                    {
                        Screen.Append(" ");
                        continue;
                    }

                    if (block == 1)
                    {
                        if (row == 1 || row == blockHeight)
                            Screen.Append("-");
                        else if (col == 1 || col == blockWidth)
                            Screen.Append("|");
                        else if (row == 2 && col > 1 && nameIdx < deck.Length)
                            Screen.Append(deck[nameIdx++]);
                        else if (row == (blockHeight + 1) / 2 &&
                                 col >= (blockWidth - deckCount.ToString().Length + 1) / 2 &&
                                 nameIdx < deckCount.ToString().Length)
                            Screen.Append(deckCount.ToString()[nameIdx++]);
                        else
                            Screen.Append(" ");
                    }
                    else
                    {
                        if (row == 1 || row == blockHeight)
                            Screen.Append("-");
                        else if (col == 1 || col == blockWidth)
                            Screen.Append("|");
                        else if (row == 2 && col > 1 && nameIdx < cementery.Length)
                            Screen.Append(cementery[nameIdx++]);
                        else if (row == (blockHeight + 1) / 2 &&
                                 col >= (blockWidth - cementeryCount.ToString().Length + 1) / 2 &&
                                 nameIdx < cementeryCount.ToString().Length)
                            Screen.Append(cementeryCount.ToString()[nameIdx++]);
                        else
                            Screen.Append(" ");
                    }
                }

                if (block != 5)
                    for (int i = 1; i <= tabSeparation; i++)
                        Screen.Append(" ");
            }

            Screen.AppendLine();
        }
    }

    static void PrintHeroes(int blockWidth, int blockHeight, int tabSeparation, SimplePlayer player)
    {
        for (int row = 1; row <= blockHeight; row++)
        {
            for (int block = 1; block <= 5; block++)
            {
                HeroCard? hero = player.HeroZone.Count >= block ? player.HeroZone[block - 1] : null;

                string heroName = "👾None👾";

                int nameIdx = 0;

                int attack = 0;
                int defense = 0;

                if (hero != null)
                {
                    attack = hero.Attack;
                    defense = hero.Defense;
                    heroName = $"👾{hero.Name}👾";
                }

                string attDeff = $"att: {attack} deff: {defense}";

                int attIdx = 0;

                for (int col = 1; col <= blockWidth; col++)
                {
                    if (row == 1 || row == blockHeight)
                        Screen.Append("-");
                    else if ((col == 1 || col == blockWidth))
                        Screen.Append("|");
                    else if (row == 2 && col >= (blockWidth - heroName.Length + 1) / 2 && nameIdx < heroName.Length)
                        Screen.Append(heroName[nameIdx++]);
                    else if (row == 3 && attIdx < attDeff.Length && hero != null)
                        Screen.Append(attDeff[attIdx++]);
                    else
                        Screen.Append(" ");
                }

                if (block == 5) continue;
                for (int i = 1; i <= tabSeparation; i++)
                    Screen.Append(" ");
            }

            Screen.AppendLine();
        }
    }

    static void PrintItems(int blockWidth, int blockHeight, int tabSeparation, SimplePlayer player)
    {
        for (int row = 1; row <= blockHeight; row++)
        {
            for (int block = 1; block <= 5; block++)
            {
                ItemCard? item = player.ItemZone.Count >= block ? player.ItemZone[block - 1] : null;

                string itemName = "⚔️None⚔️";

                if (item != null)
                {
                    itemName = $"⚔️{item.Name}⚔️";
                }

                int nameIdx = 0;

                for (int col = 1; col <= blockWidth; col++)
                {
                    if (row == 1 || row == blockHeight)
                        Screen.Append("-");
                    else if ((col == 1 || col == blockWidth))
                        Screen.Append("|");
                    else if ((row == (blockHeight + 1) / 2) && col >= (blockWidth - itemName.Length + 1) / 2 &&
                             nameIdx < itemName.Length)
                        Screen.Append(itemName[nameIdx++]);
                    else
                        Screen.Append(" ");
                }

                if (block != 5)
                    for (int i = 1; i <= tabSeparation; i++)
                        Screen.Append(" ");
            }

            Screen.AppendLine();
        }
    }



    public static void PrintHelp()
    {

        if (_helpMenu == "")
        {
            _helpMenu = @"
            -----------------------HELP----------------------------
            | a[0-4] : Attacks with the selected Hero              |
            | ch     : Activates the Effect of the Selected Hero   |
            | ci     : Activates the Effect of the Selected Item   |
            | dh     : Unsets the current selected Hero            |
            | di     : Unsets the current selected item            |
            | e      : Ends the Turn                               |
            | h      : Toggle help menu                            |
            | i      : Invoke selected Card in Hand if it's a Hero |
            | i[0-4] : Equip selected Item in Hand if it's a Item  |
            | sc#⏎   : Select the card at Position in Hand         |
            | sh[0-4]: Select Hero at the Position in HeroZone     |
            | si[0-4]: Select Item at the Position in ItemZone     |
            | vc     : View the Selected card in Hand              |
            | vh     : View the Selected Hero                      |
            | vi     : View the Selected Item                      |
            | vv     : View Player's Hand                          |
            -----------------------HELP----------------------------
            ";
        }

        if (_helpActive)
        {
            Console.Clear();
            Console.Write(Screen);

        }
        else
        {
            Console.Clear();
            Console.Write(_helpMenu);
        }

        _helpActive = !_helpActive;
    }

    public static void ViewCardInfo(SimpleCard card)
    {
        Console.Clear();
        if (!_viewInfo)
        {
            Console.WriteLine("               NAME                 ");
            Console.WriteLine($"            {card.Name}            ");
            Console.WriteLine("\n\n          Description            ");
            Console.WriteLine(card.Description);
            Console.WriteLine("\n\n            Condition                ");
            Console.WriteLine(card.Effect.ConditionString);

            Console.WriteLine("\n\n            Action              ");
            Console.WriteLine(card.Effect.ActionString);

            if (card is HeroCard hero)
            {
                Console.WriteLine("             Attack              ");
                Console.WriteLine($"             {hero.Attack}");
                Console.WriteLine("             Defense              ");
                Console.WriteLine($"             {hero.Defense}");

            }
        }
        else
        {
            Console.Write(Screen);
        }

        _viewInfo = !_viewInfo;

    }

    public static void PrintHand(SimplePlayer player)
    {
        _viewInfo = !_viewInfo;
        PrintHand(player, _viewInfo);
    }


    public static void PrintHand(SimplePlayer player, bool display)
    {
        if (!display)
        {
            Console.Clear();
            Console.Write(Screen);

            return;

        }


        const int tabSeparation = 5;

        var blockWidth = (Console.LargestWindowWidth - tabSeparation * 4) / 5;

        int blockHeight = 6;

        int handSize = player.Hand.Count;

        int cardIdx = 0;

        int rowCount = 0;

        StringBuilder hand = new StringBuilder();

        while (cardIdx < handSize)
        {
            for (int row = 1; row <= blockHeight; row++)
            {
                for (int block = 1; block <= 5; block++)
                {
                    var card = player.Hand.Count > cardIdx ? player.Hand[cardIdx] : null;

                    string cardName = card == null ? "CardName" : card.Name;

                    int nameIdx = 0;

                    for (int col = 1; col <= blockWidth; col++)
                    {
                        if (rowCount * 5 + block > handSize)
                        {
                            hand.Append(" ");
                            continue;
                        }
                        if (row == 1 || row == blockHeight)
                            hand.Append("-");
                        else if ((col == 1 || col == blockWidth))
                            hand.Append("|");
                        else if ((row == 2) && col >= (blockWidth - cardName.Length + 1) / 2 &&
                                 nameIdx < cardName.Length)
                        {
                            hand.Append(cardName[nameIdx++]);
                            if (nameIdx == cardName.Length) cardIdx++;
                        }

                        else
                            hand.Append(" ");
                    }

                    if (block != 5)
                        for (int i = 1; i <= tabSeparation; i++)
                            hand.Append(" ");
                }

                hand.AppendLine();
            }

            rowCount++;
        }

        Console.Clear();
        Console.Write(hand);
    }
}