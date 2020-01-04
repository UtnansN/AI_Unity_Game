using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using DefaultNamespace;
using UnityEngine.WSA;

public class TileProp
{
    public Team Team;
    public TileState State;

    public TileProp()
    {
        Team = Team.None;
        State = TileState.Base;
    }

    public TileProp(Team team, TileState state)
    {
        Team = team;
        State = state;
    }

    public static TileProp[,] CreateStartArray(int size)
    {
        var arr = new TileProp[size, size];
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                arr[i, j] = new TileProp();
            }
        }
        return arr;
    }

    public TileProp Clone()
    {
        return new TileProp(Team, State);    
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
    
    protected bool Equals(TileProp other)
    {
        return Team == other.Team && State == other.State;
    }
    
    public override int GetHashCode()
    {
        unchecked
        {
            return ((int) Team * 397) ^ (int) State;
        }
    }
    
}

public class TreeNode
{
    public TileProp[,] Tiles;
    public Dictionary<string, TreeNode> Children = new Dictionary<string, TreeNode>();
    
    private GameState _gameState;
    private Role _role;
    private TreeNode _parent;
    
    public TreeNode(GameState state, Role role, TreeNode parent)
    {
        _gameState = state;
        _role = role;
        _parent = parent;
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
                    if (currentTile.Team != Team.None &&
                        (currentTile.Team != team || currentTile.State != TileState.Base)) continue;
                    
                    var newNode = new TreeNode(nextState, nextRole, this);

                    var changedTile = new TileProp(currentTile.Team == Team.None ? team : currentTile.Team, 
                        currentTile.State == TileState.Base ? currentTile.Team == Team.None ? TileState.Base : TileState.Upgraded : TileState.Base);

                    var newTiles = TileProp.CloneArray(Tiles);
                    newTiles[i, j] = changedTile;

                    newNode.Tiles = newTiles;
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
                var newNode = new TreeNode(nextState, nextRole, this);
                var newTiles = TileProp.CloneArray(Tiles);
                Board.ShiftTiles(direction, newTiles);

                newNode.Tiles = newTiles;
                Children.Add(direction.ToString(), newNode);
            }
        }

        if (layers <= 1) return;
        
        foreach (var child in Children.Values)
        {
            child.GenerateChildren(layers - 1);
        }
    }
}
