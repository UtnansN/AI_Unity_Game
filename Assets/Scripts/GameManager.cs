using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public Board board;
    public StateTextHandler stateTextHandler;
    public Toggle humanToggle;
    
    public enum State
    {
        Placement,
        Movement
    }
    
    public static State GameState = State.Placement;
    
    
    void Start()
    {
        ResetGame();
    }

    private void ResetGame()
    {
        board.InitBoard();
        stateTextHandler.SetDefaultStateText();
        GameState = State.Placement;

        if (humanToggle.isOn)
        {
            
        }
    }

}
