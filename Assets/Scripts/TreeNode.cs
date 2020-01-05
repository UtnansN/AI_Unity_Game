using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.WSA;

public class TileProp
{
    public Team Team;

    public TileProp()
    {
        Team = Team.None;
    }

    public TileProp(Team team)
    {
        Team = team;
    }

    public TileProp Clone()
    {
        return new TileProp(Team);    
    }

    public static TileProp[,] CloneArray(TileProp[,] other)
    {
        var size = other.GetLength(0);
        var arr = new TileProp[size, size];

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                arr[i, j] = other[i, j].Clone();
            }
        }

        return arr;
    }
    
    public bool Equals(TileProp other)
    {
        return Team == other.Team;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return ((int) Team * 397);
        }
    }
    
}

public class TreeNode
{
    public TileProp[,] Tiles;
    public Dictionary<string, TreeNode> Children = new Dictionary<string, TreeNode>();
    
    private GameState _gameState;
    private Role _role;

    [HideInInspector]
    public bool isFinalState;


    public TreeNode(GameState state, Role role)
    {
        _gameState = state;
        _role = role;
    }

    public void GenerateChildren(int layers)
    {
        var nextRole = _role == Role.Minimizer ? Role.Maximizer : Role.Minimizer;
        var gameLength = Tiles.GetLength(0);

        if (_gameState == GameState.Placement)
        {
            var nextState = _role == Role.Minimizer ? GameState.Movement : GameState.Placement;
            var team = _role == CpuPlayer.Instance.role ? Team.Cpu : Team.Human;
            

            // Generate placement children
            for (var i = 0; i < gameLength; i++)
            {
                for (var j = 0; j < gameLength; j++)
                {
                    var currentTile = Tiles[i, j];
                    
                    // Checks if tile is valid for branching
                    if (currentTile.Team != Team.None) continue;
                    
                    var newNode = new TreeNode(nextState, nextRole);

                    var changedTile = new TileProp(currentTile.Team == Team.None ? team : currentTile.Team);

                    var newTiles = TileProp.CloneArray(Tiles);
                    newTiles[i, j] = changedTile;

                    var currLayer = CpuPlayer.Instance.treeDepth - layers;
                    var existingGameState = ReturnIfExistsEqualNode(newTiles, currLayer);
                    if (existingGameState != null)
                    {
                        newNode = existingGameState;
                    }
                    else
                    {
                        newNode.Tiles = newTiles;
                        CpuPlayer.Instance.layers[currLayer].Add(newNode);
                    }
                    
                    Children.Add(i + " " + j, newNode);
                }
            }
        }
        else
        {
            var nextState = _role == Role.Minimizer ? GameState.Placement : GameState.Movement;
            
            // Generate movement children
            foreach (var direction in Enum.GetValues(typeof(MovementDirection)).Cast<MovementDirection>())
            {
                var newNode = new TreeNode(nextState, nextRole);
                var newTiles = TileProp.CloneArray(Tiles);
                Board.ShiftTiles(direction, newTiles);

                var currLayer = CpuPlayer.Instance.treeDepth - layers;
                var existingGameState = ReturnIfExistsEqualNode(newTiles, currLayer);
                if (existingGameState != null)
                {
                    newNode = existingGameState;
                }
                else
                {
                    newNode.Tiles = newTiles;
                    CpuPlayer.Instance.layers[currLayer].Add(newNode);
                }
                
                Children.Add(direction.ToString(), newNode);
            }
        }

        isFinalState = Children.Count == 0;
    }

    private static TreeNode ReturnIfExistsEqualNode(TileProp[,] comparable, int layerIndex)
    {
        TreeNode returnable = null;
        foreach (var node in CpuPlayer.Instance.layers[layerIndex])
        {
            var workTiles = node.Tiles;
            var equal = true;

            for (var i = 0; i < workTiles.GetLength(0); i++)
            {
                for (var j = 0; j < workTiles.GetLength(1); j++)
                {
                    if (workTiles[i, j].Equals(comparable[i, j])) continue;

                    equal = false;
                    break;
                }

                if (!equal) break;
            }
            
            if (!equal) continue;
            
            returnable = node;
            break;
        }
        return returnable;
    }

    public void UpdateLayers(int layer)
    {
        if (layer >= CpuPlayer.Instance.treeDepth) return;

        var currLayer = CpuPlayer.Instance.layers[layer];
        
        foreach (var child in Children.Values.Where(child => !currLayer.Contains(child)))
        {
            currLayer.Add(child);
            child.UpdateLayers(layer + 1);
        }
    }

    private int EvaluateNode()
    {
        var (item1, item2) = GameManager.EvaluateScores(Tiles);

        if (CpuPlayer.Instance.role == Role.Maximizer) return item2 - item1;
        return item1 - item2;
    }

    public (int, TreeNode) AlphaBetaPrune(int alphaValue, int betaValue)
    {
        if (Children.Count == 0) return (EvaluateNode(), this);
        int bestValue;
        TreeNode bestNode = null;
        
        if (_role == Role.Maximizer)
        {
            bestValue = int.MinValue;
            foreach (var child in Children.Values)
            {
                var result = child.AlphaBetaPrune(alphaValue, betaValue);
                if (result.Item1 > bestValue)
                {
                    bestValue = result.Item1;
                    bestNode = child;
                }

                alphaValue = Math.Max(bestValue, alphaValue);
                if (alphaValue >= betaValue) break;
            }
        }
        else
        {
            bestValue = int.MaxValue;
            foreach (var child in Children.Values)
            {
                var result = child.AlphaBetaPrune(alphaValue, betaValue);
                if (result.Item1 < bestValue)
                {
                    bestValue = result.Item1;
                    bestNode = child;
                }

                betaValue = Math.Min(bestValue, betaValue);
                if (alphaValue >= betaValue) break;
            }
        }
        
        return (bestValue, bestNode);
    }
}
