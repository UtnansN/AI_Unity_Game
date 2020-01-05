using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class StateTextHandler : MonoBehaviour
{

    public Text phaseText;
    public Text humanCounter;
    public Text cpuCounter;

    private const string PlacementText = "Ievietošanas fāze";
    private const string MovementText = "Kustības fāze";    
    
    public void SetDefaultPoints()
    {
        humanCounter.text = "0";
        cpuCounter.text = "0";
    }

    public void AdjustStateAndPoints()
    {
        phaseText.text = GameManager.Instance.gameState == GameState.Placement ? PlacementText : MovementText;
        var points = GameManager.EvaluateScores(GameManager.Instance.board.ToTileProp());

        humanCounter.text = points.Item1.ToString();
        cpuCounter.text = points.Item2.ToString();
    }

}
