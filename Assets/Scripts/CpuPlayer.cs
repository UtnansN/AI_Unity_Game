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
        GameManager.Instance.PerformPlacement(0, 0, false);
        Navigate(_root.Children[0 + " " + 0]);
    }
    
    private void DoMovement()
    {
        // Get best move
        GameManager.Instance.PerformMovement(MovementDirection.Right, false);
        Navigate(_root.Children["Right"]);
    }

}
