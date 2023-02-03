using Player;

namespace GameLoop;

///<summary>
///The game loop
///</summary>
public class GameLoop
{
    ///<summary>
    ///The players in the game
    ///</summary>
    SimplePlayer[] _players { get; set; }

    ///<summary>
    ///Creates a new `GameLoop`
    ///</summary>
    ///<param name="players">The players in the game</param>
    public GameLoop(params SimplePlayer[] players)
    {
        this._players = players;
    }

    ///<summary>
    ///Starts the game
    ///</summary>
    public void StartGame()
    {
        bool initialTurn = true;

        while (true)
        {
            foreach (SimplePlayer player in _players)
            {
                if (CheckPlayerHasLost(player))
                {
                    EndGame();
                    return;
                }

                if (player is AIPlayer)
                {
                    AIPlayer aiPlayer = (AIPlayer)player;
                    aiPlayer.DrawCards(initialTurn ? 5 : 1);
                    aiPlayer.Play();
                }
            }
            initialTurn = false;
        }

    }

    ///<summary>
    ///Ends the game and prints the winning player
    ///</summary>
    public void EndGame()
    {
        foreach (SimplePlayer player in _players)
        {
            if (CheckPlayerHasLost(player))
            {
                System.Console.WriteLine($"{player.Name} has lost");
            }
            else
            {
                System.Console.WriteLine($"{player.Name} has won");
            }
        }
    }

    ///<summary>
    ///Checks whether a player has lost because of his HP or his hand
    ///</summary>
    ///<param name="player">The player to check</param>
    ///<returns>True if the player has lost, false otherwise</returns>
    public bool CheckPlayerHasLost(SimplePlayer player)
    {
        if (player.HP == 0 || player.Hand.Count == 0)
            return true;

        return false;
    }
}