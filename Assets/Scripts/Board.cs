using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Board : MonoBehaviour
{

    public int size = 4;
    public GameObject tilePrefab;
    
    [HideInInspector]
    public GameObject[,] tiles;

    public void InitBoard()
    {
        if (tiles != null)
        {
            CleanUpPrevious();
        }
        
        tiles = new GameObject[size,size];

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                GameObject gameTile = Instantiate(tilePrefab, transform);


                BoardTile tileComponent = gameTile.GetComponent<BoardTile>();
                tileComponent.row = i;
                tileComponent.column = j;
                
                gameTile.GetComponent<Button>().onClick.AddListener(() => { GameManager.Instance.PerformPlacement(tileComponent.row, tileComponent.column, true); });
                tiles[i, j] = gameTile;
            }
        }
    }

    private void CleanUpPrevious()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                Destroy(tiles[i, j]);
            }
        }
    }

    public void DisableTiles()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                tiles[i, j].GetComponent<Button>().enabled = false;
            }
        }
    }

    public void EnableClickableTiles()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                var boardTile = tiles[i, j].GetComponent<BoardTile>();
                if ((boardTile.team == Team.Human && boardTile.tileState == TileState.Base) || boardTile.team == Team.None)
                {
                    tiles[i, j].GetComponent<Button>().enabled = true;
                }
            }
        }
    }
    
    public BoardTile FetchTile(int x, int y)
    {
        return tiles[x, y].GetComponent<BoardTile>();
    }

    public void ApplyTileProp(TileProp[,] tileProps)
    {
        for (var i = 0; i < size; i++)
        {
            for (var j = 0; j < size; j++)
            {
                var current = FetchTile(i, j);
                current.team = tileProps[i, j].Team;
                current.tileState = tileProps[i, j].State;
            }
        }
    }

    public TileProp[,] ToTileProp()
    {
        var boardSize = size;
        var tileProps = new TileProp[boardSize, boardSize];
        for (var i = 0; i < boardSize; i++)
        {
            for (var j = 0; j < boardSize; j++)
            {
                var boardTile = tiles[i, j].GetComponent<BoardTile>();
                tileProps[i, j] = new TileProp(boardTile.team, boardTile.tileState);
            }
        }
        return tileProps;
    }

    public static TileProp[,] ShiftTiles(MovementDirection direction, TileProp[,] tileProps)
    {
        switch (direction)
        {
            case MovementDirection.Up:
                return ShiftUp(tileProps);
            case MovementDirection.Down:
                return ShiftDown(tileProps);
            case MovementDirection.Left:
                return ShiftLeft(tileProps);
            case MovementDirection.Right:
                return ShiftRight(tileProps);
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }

    private static TileProp[,] ShiftUp(TileProp[,] tileProps)
    {
        var size = GameManager.Instance.board.size;
        // Column movement up
        for (var j = 0; j < size; j++)
        {
            var lastFreeIndex = -1;
            for (var i = 0; i < size ; i++)
            {
                var tile = tileProps[i, j];
                if (tile.Team == Team.None)
                {
                    lastFreeIndex = i;
                } else if (tile.State == TileState.Upgraded && lastFreeIndex != -1)
                {
                    for (var x = lastFreeIndex; x < i ; x++)
                    {
                        var currTile = tileProps[x, j];
                        var adjacentTile = tileProps[x + 1, j];

                        currTile.Team = adjacentTile.Team;
                        currTile.State = adjacentTile.State;
                    }

                    tile.Team = Team.None;
                    tile.State = TileState.Base;
                    lastFreeIndex = i;
                }
            }
        }

        return tileProps;
    }

    private static TileProp[,] ShiftDown(TileProp[,] tileProps)
    {
        var size = GameManager.Instance.board.size;
        // Column movement down
        for (var j = 0; j < size; j++)
        {
            var lastFreeIndex = -1;
            for (var i = size - 1; i >= 0 ; i--)
            {
                var tile = tileProps[i, j];
                if (tile.Team == Team.None)
                {
                    lastFreeIndex = i;
                } else if (tile.State == TileState.Upgraded && lastFreeIndex != -1)
                {
                    for (var x = lastFreeIndex; x > i ; x--)
                    {
                        var currTile = tileProps[x, j];
                        var adjacentTile = tileProps[x - 1, j];

                        currTile.Team = adjacentTile.Team;
                        currTile.State = adjacentTile.State;
                    }

                    tile.Team = Team.None;
                    tile.State = TileState.Base;
                    lastFreeIndex = i;
                }
            }
        }

        return tileProps;
    }

    private static TileProp[,] ShiftLeft(TileProp[,] tileProps)
    {
        var size = GameManager.Instance.board.size;
        // Row movement left
        for (var i = 0; i < size; i++)
        {
            var lastFreeIndex = -1;
            for (var j = 0; j < size ; j++)
            {
                var tile = tileProps[i, j];
                if (tile.Team == Team.None)
                {
                    lastFreeIndex = j;
                } else if (tile.State == TileState.Upgraded && lastFreeIndex != -1)
                {
                    for (var x = lastFreeIndex; x < j; x++)
                    {
                        var currTile = tileProps[i, x];
                        var adjacentTile = tileProps[i, x + 1];

                        currTile.Team = adjacentTile.Team;
                        currTile.State = adjacentTile.State;
                    }

                    tile.Team = Team.None;
                    tile.State = TileState.Base;
                    lastFreeIndex = j;
                }
            }
        }

        return tileProps;
    }

    private static TileProp[,] ShiftRight(TileProp[,] tileProps)
    {
        var size = GameManager.Instance.board.size;
        for (var i = 0; i < size; i++)
        {
            var lastFreeIndex = -1;
            for (var j = size - 1; j >= 0 ; j--)
            {
                var tileComponent = tileProps[i, j];
                if (tileComponent.Team == Team.None)
                {
                    lastFreeIndex = j;
                } else if (tileComponent.State == TileState.Upgraded && lastFreeIndex != -1)
                {
                    for (var x = lastFreeIndex; x > j; x--)
                    {
                        var currTile = tileProps[i, x];
                        var adjacentTile = tileProps[i, x - 1];

                        currTile.Team = adjacentTile.Team;
                        currTile.State = adjacentTile.State;
                    }

                    tileComponent.Team = Team.None;
                    tileComponent.State = TileState.Base;
                    lastFreeIndex = j;
                }
            }
        }

        return tileProps;
    }

    public void RefreshTiles()
    {
        foreach (var tile in tiles)
        {
            tile.GetComponent<BoardTile>().RefreshSprite();
        }
    }
}
