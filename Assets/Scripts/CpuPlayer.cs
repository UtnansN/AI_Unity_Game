using DefaultNamespace;
using UnityEngine;

public class CpuPlayer : MonoBehaviour
{
    public static CpuPlayer Instance { get; private set; }

    public int treeDepth = 10;

    [HideInInspector]
    public Role role;
    
    private TreeNode _root;

    private void Start()
    {
        Instance = this;
    }
    
    public void GenerateGameTree(bool isFirst)
    {
        role = isFirst ? Role.Maximizer : Role.Minimizer;

        _root = new TreeNode(GameState.Placement, Role.Maximizer, null)
        {
            Tiles = TileProp.CreateStartArray(GameManager.Instance.board.size)
        };

        _root.GenerateChildren(treeDepth);
    }

    public void Notify(int x, int y)
    {
        
    }

    public void Notify(MovementDirection direction)
    {
        
    }

}
