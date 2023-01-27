using Game;
namespace GameLoop;

public interface IGameLoop
{
    void StartGame();
    void EndGame();
    void Update();
    void CheckEndGame();

}

public class GameLoop : IGameLoop
{
    public Game GamePlay { get; }
    public AIPlayer Player1 { get; }
    public AIPlayer Player2 { get; }

    public GameLoop(Game gamePlay, AIPlayer player1, AIPlayer player2)
    {
        GamePlay = gamePlay;
        Player1 = player1;
        Player2 = player2;
    }

    public void StartGame()
    {
        bool player1Turn = true;
        bool gameStart = false;
        while (!CheckEndGame())
        {
            if (!gameStart)
            {
                gameStart = true;
                GamePlay.NewGame(Player1, Player2);
            }
            if(player1Turn)
            {
                Player1.Play();
                player1Turn = false;
            }else
            {
                Player2.Play();
                player1Turn = true;
            }
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
        if(Player1.HP <= 0 || Player2.HP <= 0)
        {
            return true;
        }
        if (player)
        {
            if (Player1.Deck.Count == 0)
            {
                return true;
            }
        }
        else
        {
            if (Player2.Deck.Count == 0)
            {
                return true;
            }
        }
        return false;
    }
}
