using Player;
using Cards;

namespace GameLoop;


public class Printer
{


    public static void Print(SimplePlayer p1, SimplePlayer p2)
    {

        Console.Clear();

        int tabSeparation = 5;

        int blockWidth = (Console.LargestWindowWidth - tabSeparation * 4) / 5;

        int blockHeight = 5;

        Console.Title = "Game";

        PrintPlayer(blockWidth, tabSeparation, p1.Name, $"❤️  {p1.HP}  ❤️");
        PrintDeckAndCementery(blockWidth, blockHeight, tabSeparation, p1);
        PrintItems(blockWidth, 3, tabSeparation, p1);
        PrintHeroes(blockWidth, blockHeight, tabSeparation, p1);

        PrintHeroes(blockWidth, blockHeight, tabSeparation, p2);
        PrintItems(blockWidth, 3, tabSeparation, p2);
        PrintDeckAndCementery(blockWidth, blockHeight, tabSeparation, p2, true);
        PrintPlayer(blockWidth, tabSeparation, $"❤️  {p2.HP}  ❤️", p2.Name);


    }

    static void PrintPlayer(int blockWidth, int tabSeparation, string buff1, string buff2)
    {

        Console.Write(buff1);

        int usedWidth = blockWidth * 5 + tabSeparation * 4;

        for (int i = buff1.Length; i < usedWidth - buff2.Length; i++)
        {
            Console.Write(" ");
        }

        Console.WriteLine(buff2);
    }


    static void PrintDeckAndCementery(int blockWidth, int blockHeight, int tabSeparation, SimplePlayer player, bool invert = false)
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
                    if (block > 1 && block < 5) { Console.Write(" "); continue; }

                    if (block == 1)
                    {
                        if (row == 1 || row == blockHeight)
                            Console.Write("-");
                        else if (row != 1 && (col == 1 || col == blockWidth))
                            Console.Write("|");
                        else if (row == 2 && col > 1 && nameIdx < deck.Length)
                            Console.Write(deck[nameIdx++]);
                        else if ((row == (blockHeight + 1) / 2) && col >= (blockWidth - deckCount.ToString().Length + 1) / 2 && nameIdx < deckCount.ToString().Length)
                            Console.Write(deckCount.ToString()[nameIdx++]);
                        else
                            Console.Write(" ");
                    }
                    else
                    {
                        if (row == 1 || row == blockHeight)
                            Console.Write("-");
                        else if (row != 1 && (col == 1 || col == blockWidth))
                            Console.Write("|");
                        else if (row == 2 && col > 1 && nameIdx < cementery.Length)
                            Console.Write(cementery[nameIdx++]);
                        else if ((row == (blockHeight + 1) / 2) && col >= (blockWidth - cementeryCount.ToString().Length + 1) / 2 && nameIdx < cementeryCount.ToString().Length)
                            Console.Write(cementeryCount.ToString()[nameIdx++]);
                        else
                            Console.Write(" ");
                    }

                }
                if (block != 5)
                    for (int i = 1; i <= tabSeparation; i++) Console.Write(" ");
            }
            System.Console.WriteLine();
        }
    }

    public static void PrintHeroes(int blockWidth, int blockHeight, int tabSeparation, SimplePlayer player)
    {
        for (int row = 1; row <= blockHeight; row++)
        {
            for (int block = 1; block <= 5; block++)
            {
                HeroCard? hero = (player.HeroZone.Count >= block) ? player.HeroZone[block - 1] : null;

                string heroName = $"👾None👾";

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
                        Console.Write("-");
                    else if (row != 1 && (col == 1 || col == blockWidth))
                        Console.Write("|");
                    else if (row == 2 && col >= (blockWidth - heroName.Length + 1) / 2 && nameIdx < heroName.Length)
                        Console.Write(heroName[nameIdx++]);
                    else if (row == 3 && attIdx < attDeff.Length && hero != null)
                        Console.Write(attDeff[attIdx++]);
                    else
                        Console.Write(" ");

                }
                if (block != 5)
                    for (int i = 1; i <= tabSeparation; i++) Console.Write(" ");
            }
            System.Console.WriteLine();

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
                        Console.Write("-");
                    else if (row != 1 && (col == 1 || col == blockWidth))
                        Console.Write("|");
                    else if ((row == (blockHeight + 1) / 2) && col >= (blockWidth - itemName.Length + 1) / 2 && nameIdx < itemName.Length)
                        Console.Write(itemName[nameIdx++]);
                    else
                        Console.Write(" ");

                }
                if (block != 5)
                    for (int i = 1; i <= tabSeparation; i++) Console.Write(" ");
            }
            System.Console.WriteLine();

        }
    }
}



