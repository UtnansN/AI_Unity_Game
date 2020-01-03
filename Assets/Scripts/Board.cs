using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Board : MonoBehaviour
{

    public int size = 6;
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
                if (boardTile.team == Team.Human && boardTile.tileState == TileState.Base)
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

    public void ShiftTiles(MovementDirection direction)
    {
        // TODO
    }
    
}
