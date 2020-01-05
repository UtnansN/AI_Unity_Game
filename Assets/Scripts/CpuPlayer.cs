using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

public class CpuPlayer : MonoBehaviour
{
    public static CpuPlayer Instance { get; private set; }

    public int treeDepth = 6;

    [HideInInspector] public List<List<TreeNode>> layers;

    [HideInInspector]
    public Role role;
    
    private TreeNode _root;

    private void Start()
    {
        Instance = this;
    }

    private void EmptyLayerList()
    {
        layers = new List<List<TreeNode>>();
        for (int i = 0; i < treeDepth; i++)
        {
            layers.Add(new List<TreeNode>());
        }
    }
    
    public void GenerateGameTree(bool isFirst)
    {
        EmptyLayerList();
        role = isFirst ? Role.Maximizer : Role.Minimizer;

        _root = new TreeNode(GameState.Placement, Role.Maximizer)
        {
            Tiles = TileProp.CreateStartArray(GameManager.Instance.board.size)
        };

        _root.GenerateChildren(treeDepth);
        for (var i = 0; i < treeDepth - 1; i++)
        {
            foreach (var node in layers[i])
            {
                node.GenerateChildren(treeDepth - 1 - i);
            }
        }

        if (role == Role.Maximizer)
        {
            DoPlacement();
        }
    }

    public void Notify(int x, int y)
    {
        var newNode = _root.Children[x + " " + y];
        Navigate(newNode);

        if (role == Role.Maximizer)
        {
            DoMovement();
        }
        else
        {
            DoPlacement();
        }
    }

    public void Notify(MovementDirection direction)
    {
        var newNode = _root.Children[direction.ToString()];
        Navigate(newNode);

        if (role == Role.Maximizer)
        {
            DoPlacement();
        }
        else
        {
            DoMovement();
        }
    }

    private void Navigate(TreeNode newNode)
    {
        _root = newNode;
        if (_root.isFinalState)
        {
            Debug.Log("Game over");
            return;
        }
        
        
        EmptyLayerList();
        _root.UpdateLayers(0);
        foreach (var child in layers[treeDepth - 2]) 
        {
            child.GenerateChildren(1);
        }
    }
    
    private void DoPlacement()
    {
        // Get best move
        TreeNode bestNode;
        (_, bestNode) = _root.AlphaBetaPrune(int.MinValue, int.MaxValue);

        var key = _root.Children.FirstOrDefault(x => x.Value == bestNode).Key;
        var xy = key.Split(' ');
        
        GameManager.Instance.PerformPlacement(int.Parse(xy[0]), int.Parse(xy[1]), false);
        Navigate(bestNode);
    }
    
    private void DoMovement()
    {
        // Get best move
        TreeNode bestNode;
        (_, bestNode) = _root.AlphaBetaPrune(int.MinValue, int.MaxValue);
        
        var directionKey = _root.Children.FirstOrDefault(x => x.Value == bestNode).Key;

        var direction = MovementDirection.Up;
        
        switch (directionKey)
        {
            case "Up":
                direction = MovementDirection.Up;
                break;
            case "Down":
                direction = MovementDirection.Down;
                break;
            case "Left":
                direction = MovementDirection.Left;
                break;
            case "Right":
                direction = MovementDirection.Right;
                break;
        }
        
        GameManager.Instance.PerformMovement(direction, false);
        Navigate(bestNode);
    }

    

}
