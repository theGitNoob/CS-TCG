using Game;
using Player;

namespace GameLoop;

public interface IGameLoop
{
    void StartGame();
    void EndGame();
    void Update();
    void CheckEndGame();
}

public class GameLoop
{
    public GameLoop()
    {
        // Player1 = player1;
        // Player2 = player2;
    }

    public void StartGame()
    {
        bool player1Turn = true;
        bool gameStart = false;
        while (!CheckEndGame(player1Turn))
        {
            if (!gameStart)
            {
                gameStart = true;
                GameController.NewGame();
            }

            if (player1Turn)
            {
                GameController.RetrievePlayer(1).Play();
                player1Turn = false;
            }
            else
            {
                GameController.RetrievePlayer(2).Play();
                player1Turn = true;
            }

            Task.Delay(1000);
        }
    }

    public void EndGame()
    {
        throw new System.NotImplementedException();
    }

    public void Update()
    {
        throw new System.NotImplementedException();
    }

    public bool CheckEndGame(bool player)
    {
        if (GameController.RetrievePlayer(1).HP <= 0 || GameController.RetrievePlayer(2).HP <= 0)
            return true;

        switch (player)
        {
            case true when GameController.RetrievePlayer(1).Deck.Cards.Count == 0:
            case false when GameController.RetrievePlayer(2).Deck.Cards.Count == 0:
                return true;
            default:
                return false;
        }
    }
}