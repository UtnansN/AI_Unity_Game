using DefaultNamespace;
using UnityEngine;

public class CpuPlayer : MonoBehaviour
{

    public bool IsFirst { get; private set; }
    
    public void GenerateGameTree(bool first)
    {
        IsFirst = first;
        
        // TODO
    }

    public void Notify(int x, int y)
    {
        
    }

    public void Notify(MovementDirection direction)
    {
        
    }

}
