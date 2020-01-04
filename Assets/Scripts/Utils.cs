namespace DefaultNamespace
{
    public enum MovementDirection
    { 
        Up, 
        Down, 
        Left, 
        Right
    }
    
    public enum TileState
    {
        Base,
        Upgraded
    }
    
    public enum Team
    {
        Human,
        Cpu,
        None
    }
    
    public enum GameState
    {
        Placement,
        Movement
    }

    public enum Role
    {
        Maximizer,
        Minimizer
    }
}