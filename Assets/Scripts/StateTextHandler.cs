using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class StateTextHandler : MonoBehaviour
{

    public Text phaseText;
    public Text humanCounter;
    public Text cpuCounter;

    private const string PlacementText = "Ievietošanas fāze";
    private const string MovementText = "Kustības fāze";    
    
    public void SetDefaultStateText()
    {
        phaseText.GetComponent<Text>().text = PlacementText;
        humanCounter.GetComponent<Text>().text = "0";
        humanCounter.GetComponent<Text>().text = "0";
    }

    public void AdjustState()
    {
        phaseText.text = GameManager.GameState == GameManager.State.Placement ? PlacementText : MovementText;
    }

}
