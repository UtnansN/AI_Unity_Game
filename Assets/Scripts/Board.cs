using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{

    public int size = 6;
    public GameObject tilePrefab;
    
    private GameObject[,] _tiles;

    public void InitBoard()
    {
        if (_tiles != null)
        {
            CleanUpPrevious();
        }
        
        _tiles = new GameObject[size,size];
        
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                GameObject gameTile = Instantiate(tilePrefab, transform);


                BoardTile tileComponent = gameTile.GetComponent<BoardTile>();
                tileComponent.row = i;
                tileComponent.column = j;
                
                gameTile.GetComponent<Button>().onClick.AddListener(() => { tileComponent.MarkTile(true); });
                _tiles[i, j] = gameTile;
            }
        }
    }

    private void CleanUpPrevious()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                Destroy(_tiles[i, j]);
            }
        }
    }

}
