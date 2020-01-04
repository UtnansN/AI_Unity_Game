using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;



public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get => _instance;
        private set => _instance = value;
    }

    public Board board;
    public StateTextHandler stateTextHandler;
    public ShiftButtonHandler shiftButtonHandler;
    public Toggle humanToggle;
    public CpuPlayer aiPlayer;

    [HideInInspector]
    public GameState gameState = GameState.Placement;

    void Start()
    {
        _instance = this;
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
        BoardTile tile = board.FetchTile(x, y);
        tile.MarkTile(isHuman);
        if (isHuman)
        {
            aiPlayer.Notify(x, y);
            SetPhase(GameState.Movement);
        }
    }

    public void PerformMovement(MovementDirection direction, bool isHuman)
    {
        board.ShiftTiles(direction);
        if (isHuman)
        {
            aiPlayer.Notify(direction);
        }
        SetPhase(GameState.Placement);
    }

    private void SetPhase(GameState state)
    {
        if (state == GameState.Movement)
        {
            gameState = GameState.Movement;
            board.DisableTiles();
            shiftButtonHandler.EnableMoveButtons();
        }
        else
        {
            gameState = GameState.Placement;
            board.EnableClickableTiles();
            shiftButtonHandler.DisableMoveButtons();
        }
        stateTextHandler.AdjustState();
    }
    
    public static Tuple<int, int> EvaluateScores(BoardTile[,] tiles)
    {
        var humanScore = 0;
        var cpuScore = 0;
        
        // Horizontal cycle
        for (var i = 0; i < tiles.GetLength(0); i++)
        {
            for (var j = 1; j < tiles.GetLength(1) - 1; j++)
            {
                var tileTeam = tiles[i, j].team;
                if (tileTeam == tiles[i, j-1].team && tileTeam == tiles[i, j+1].team)
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
                var tileTeam = tiles[i, j].team;
                if (tileTeam == tiles[i-1, j].team && tileTeam == tiles[i+1, j].team)
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
