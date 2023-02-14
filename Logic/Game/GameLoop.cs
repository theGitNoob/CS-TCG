using Player;

namespace Game;

/// <summary>
/// The game loop
/// </summary>
public static class GameLoop
{


    /// <summary>
    /// Starts the game
    /// </summary>
    public static void StartGame(int initialCards, int cardsPerTurn, SimplePlayer p1, SimplePlayer p2)
    {
        bool canAttack = false;

        p1.SetEnemy(p2);
        p2.SetEnemy(p1);

        // System.Console.WriteLine(p1.GetType());
        // System.Console.WriteLine(p2.GetType());

        p1.Deck.Shuffle();
        p2.Deck.Shuffle();

        int cardsToDraw = initialCards;

        while (true)
        {
            Printer.Print(p1, p2);

            if (CheckPlayerHasLost(p1))
            {
                EndGame(p1, p2);
                return;
            }

            p1.DrawCards(cardsToDraw);
            p1.Play(canAttack);

            Task.Delay(500).Wait();

            Printer.Print(p1, p2);

            canAttack = true;

            if (CheckPlayerHasLost(p2))
            {
                EndGame(p2, p1);
                return;
            }

            p2.DrawCards(cardsToDraw);
            p2.Play(canAttack);

            Task.Delay(500).Wait();

            cardsToDraw = cardsPerTurn;
        }
    }

    /// <summary>
    /// Ends the game and prints the winning player
    /// </summary>
    static void EndGame(SimplePlayer looser, SimplePlayer winner)
    {
        Console.WriteLine($"{looser.Name} has lost");
        Console.WriteLine($"{winner.Name} has won");
    }



    /// <summary>
    /// Checks whether a player has lost because of his HP or his hand
    /// </summary>
    /// <param name="player">The player to check</param>
    /// <returns>True if the player has lost, false otherwise</returns>
    static bool CheckPlayerHasLost(SimplePlayer player)
    {
        if (player.Hp == 0 || player.Deck.IsEmpty())
            return true;

        return false;
    }
}