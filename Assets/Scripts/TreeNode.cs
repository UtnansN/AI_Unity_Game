using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class TreeNode : MonoBehaviour
{

    [HideInInspector]
    public BoardTile[,] tiles;
    
    [HideInInspector]
    public int humanPoints = 0;
    
    [HideInInspector]
    public int cpuPoints = 0;

}
