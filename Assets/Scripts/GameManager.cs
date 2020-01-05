using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Board board;
    public StateTextHandler stateTextHandler;
    public ShiftButtonHandler shiftButtonHandler;
    public Toggle humanToggle;
    public CpuPlayer aiPlayer;

    [HideInInspector]
    public GameState gameState = GameState.Placement;
    

    void Start()
    {
        Instance = this;
        ResetGame();
    }

    public void ResetGame()
    {
        board.InitBoard();
        SetPhase(GameState.Placement);
        stateTextHandler.SetDefaultPoints();
        aiPlayer.GenerateGameTree(!humanToggle.isOn);
    }

    public void PerformPlacement(int x, int y, bool isHuman)
    {
        var tile = board.FetchTile(x, y);
        tile.MarkTile(isHuman);
        if (!isHuman) return;
        aiPlayer.Notify(x, y);
        SetPhase(GameState.Movement);
    }

    public void PerformMovement(MovementDirection direction, bool isHuman)
    {
        var tileProp = board.ToTileProp();
        board.ApplyTileProp(Board.ShiftTiles(direction, tileProp));
        board.RefreshTiles();
        if (!isHuman) return;
        aiPlayer.Notify(direction);
        SetPhase(GameState.Placement);
    }

    private void SetPhase(GameState state)
    {
        gameState = state;
        if (gameState == GameState.Movement)
        {
            board.DisableTiles();
            shiftButtonHandler.EnableMoveButtons();
        }
        else
        {
            board.EnableClickableTiles();
            shiftButtonHandler.DisableMoveButtons();
        }
        stateTextHandler.AdjustStateAndPoints();
    }
    
    public static Tuple<int, int> EvaluateScores(TileProp[,] tiles)
    {
        var humanScore = 0;
        var cpuScore = 0;
        
        // Horizontal cycle
        for (var i = 0; i < tiles.GetLength(0); i++)
        {
            for (var j = 1; j < tiles.GetLength(1) - 1; j++)
            {
                var tileTeam = tiles[i, j].Team;
                if (tileTeam == tiles[i, j-1].Team && tileTeam == tiles[i, j+1].Team)
                {
                    switch (tileTeam)
                    {
                        case Team.Human:
                            humanScore++;
                            break;
                        case Team.Cpu:
                            cpuScore++;
                            break;
                        case Team.None:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }
        
        // Vertical cycle
        for (var j = 0; j < tiles.GetLength(1); j++)
        {
            for (var i = 1; i < tiles.GetLength(0) - 1; i++)
            {
                var tileTeam = tiles[i, j].Team;
                if (tileTeam == tiles[i-1, j].Team && tileTeam == tiles[i+1, j].Team)
                {
                    switch (tileTeam)
                    {
                        case Team.Human:
                            humanScore++;
                            break;
                        case Team.Cpu:
                            cpuScore++;
                            break;
                        case Team.None:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }
        
        return new Tuple<int, int>(humanScore, cpuScore);
    }

}
