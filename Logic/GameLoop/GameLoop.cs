using Player;

namespace GameLoop;

/// <summary>
/// The game loop
/// </summary>
public class GameLoop
{
    /// <summary>
    /// The players in the game
    /// </summary>
    List<SimplePlayer> _players { get; set; }

    /// <summary>
    /// Creates a new `GameLoop`
    /// </summary>
    /// <param name="players">The players in the game</param>
    public GameLoop(List<SimplePlayer> players)
    {
        this._players = players;

        for (int curr = 0; curr < players.Count; curr++)
        {
            for (int next = curr + 1; next < players.Count; next++)
            {
                AIPlayer p1 = (AIPlayer)players[curr];
                AIPlayer p2 = (AIPlayer)players[next];

                p1.SetEnemy(p2);
                p2.SetEnemy(p1);

            }
        }
    }

    /// <summary>
    /// Starts the game
    /// </summary>
    public void StartGame()
    {
        bool canAttack = false;

        foreach (SimplePlayer player in _players)
        {
            player.Deck.Shuffle();
        }

        int cardsToDraw = 4;

        while (true)
        {
            Printer.Print(_players[0], _players[1]);

            for (int idx = 0; idx < _players.Count; idx++)
            {
                var player = _players[idx];

                //The last player is the first to attack
                if (idx == _players.Count - 1) canAttack = true;

                if (CheckPlayerHasLost(player))
                {
                    EndGame(player);
                    return;
                }

                if (player is AIPlayer)
                {
                    AIPlayer aiPlayer = (AIPlayer)player;
                    aiPlayer.DrawCards(cardsToDraw);
                    aiPlayer.Play(canAttack);
                }

                Task.Delay(500).Wait();
                Printer.Print(_players[0], _players[1]);

            }
            cardsToDraw = 1;
        }

    }

    /// <summary>
    /// Ends the game and prints the winning player
    /// </summary>
    public void EndGame(SimplePlayer looserPlayer)
    {
        foreach (SimplePlayer player in _players)
        {
            if (player == looserPlayer)
            {
                System.Console.WriteLine($"{player.Name} has lost");
            }
            else
            {
                System.Console.WriteLine($"{player.Name} has won");
            }
        }

        _players = new List<SimplePlayer>();
    }

    /// <summary>
    /// Checks whether a player has lost because of his HP or his hand
    /// </summary>
    /// <param name="player">The player to check</param>
    /// <returns>True if the player has lost, false otherwise</returns>
    public bool CheckPlayerHasLost(SimplePlayer player)
    {
        if (player.HP == 0 || player.Deck.IsEmpty())
            return true;

        return false;
    }
}